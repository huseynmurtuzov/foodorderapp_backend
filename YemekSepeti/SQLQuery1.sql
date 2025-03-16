	CREATE DATABASE YemekSepet;
	GO

	USE YemekSepet;
	GO


	CREATE TABLE Categories (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(MAX) NOT NULL
	);


	CREATE TABLE Restaurants (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(MAX) NOT NULL,
		Email NVARCHAR(MAX) NOT NULL,
		PhoneNumber NVARCHAR(MAX),
		Address NVARCHAR(MAX),
		WorkingHours NVARCHAR(MAX),
		Rating FLOAT,
		Image NVARCHAR(MAX),
		Password NVARCHAR(MAX) NOT NULL
	);


	CREATE TABLE Customers (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(MAX) NOT NULL,
		Email NVARCHAR(MAX) NOT NULL,
		PhoneNumber NVARCHAR(MAX),
		Address NVARCHAR(MAX),
		Password NVARCHAR(MAX) NOT NULL
	);

	CREATE TABLE DeliveryPersonnel (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(MAX) NOT NULL,
		Email NVARCHAR(MAX) NOT NULL,
		PhoneNumber NVARCHAR(MAX),
		Password NVARCHAR(MAX) NOT NULL,
		VehicleType NVARCHAR(MAX)
	);

	CREATE TABLE Meals (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(MAX) NOT NULL,
		Price DECIMAL(18,2) NOT NULL,
		Description NVARCHAR(MAX),
		IsAvailable BIT NOT NULL,
		Image NVARCHAR(MAX),
		Quantity INT NOT NULL,
		RestaurantId INT NOT NULL,
		FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id) ON DELETE CASCADE
	);

	CREATE TABLE CategoryMeal (
		CategoryId INT NOT NULL,
		MealId INT NOT NULL,
		PRIMARY KEY (CategoryId, MealId),
		FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE,
		FOREIGN KEY (MealId) REFERENCES Meals(Id) ON DELETE CASCADE
	);

	CREATE TABLE Orders (
		Id INT PRIMARY KEY IDENTITY(1,1),
		OrderDate DATETIME NOT NULL,
		TotalAmount DECIMAL(18,2) NOT NULL,
		Status NVARCHAR(MAX) NOT NULL,
		CustomerId INT NOT NULL,
		RestaurantId INT NOT NULL,
		DeliveryPersonnelId INT,
		FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
		FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id) ON DELETE CASCADE,
		FOREIGN KEY (DeliveryPersonnelId) REFERENCES DeliveryPersonnel(Id) ON DELETE SET NULL
	);


	CREATE TABLE MealOrder (
		MealId INT NOT NULL,
		OrderId INT NOT NULL,
		PRIMARY KEY (MealId, OrderId),
		FOREIGN KEY (MealId) REFERENCES Meals(Id) ON DELETE CASCADE,
		FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
	);


	CREATE TABLE Payments (
		Id INT PRIMARY KEY IDENTITY(1,1),
		PaymentMethod NVARCHAR(MAX) NOT NULL,
		PaymentDate DATETIME NOT NULL,
		Amount DECIMAL(18,2) NOT NULL,
		IsSuccessful BIT NOT NULL,
		OrderId INT NOT NULL,
		FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
	);


	CREATE TABLE RestaurantReviews (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Rating INT NOT NULL,
		Comment NVARCHAR(MAX),
		ReviewDate DATETIME NOT NULL,
		CustomerId INT NOT NULL,
		RestaurantId INT NOT NULL,
		FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
		FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id) ON DELETE CASCADE
	);


	CREATE TABLE CustomerRestaurant (
		CustomerId INT NOT NULL,
		RestaurantId INT NOT NULL,
		PRIMARY KEY (CustomerId, RestaurantId),
		FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
		FOREIGN KEY (RestaurantId) REFERENCES Restaurants(Id) ON DELETE CASCADE
	);



CREATE PROC up_MusteriYorumlariniAl
@CustomerId INT
AS 
BEGIN
    IF EXISTS (SELECT 1 FROM RestaurantReviews WHERE CustomerId = @CustomerId)
    BEGIN
        SELECT * FROM RestaurantReviews WHERE CustomerId = @CustomerId
    END
    ELSE
    BEGIN
        SELECT NULL AS NoReviews
    END
END



CREATE PROC up_MusteriSiparisleriniAl
@CustomerId INT
AS 
BEGIN
    IF EXISTS (SELECT 1 FROM Orders WHERE CustomerId = @CustomerId)
    BEGIN
        SELECT * FROM Orders WHERE CustomerId = @CustomerId
    END
    ELSE
    BEGIN
        SELECT NULL AS NoOrders
    END
END



CREATE PROC up_RestoranSiparisleriniAl
@RestaurantId INT
AS BEGIN
Select * from Orders as o where o.RestaurantId = @RestaurantId
END


Create PROC up_RestoranYorumlariniAl
    @RestaurantId INT
AS
BEGIN
    SELECT 
        o.Id,
        o.Comment,
        o.Rating,
        o.ReviewDate,
		o.CustomerId,
		o.RestaurantId
    FROM RestaurantReviews AS o 
    WHERE o.RestaurantId = @RestaurantId;
END


Create PROC up_MusterininSiparisSayisiniAl
@CustomerId INT
AS BEGIN
SELECT COUNT(*) FROM Orders AS o where o.CustomerId = @CustomerId
END	



CREATE TRIGGER trg_SiparisDurumunuSetEt
ON Orders
AFTER INSERT
AS
BEGIN
	UPDATE ORDERS 
	SET Status = 'Preparing'
	WHERE Id IN (SELECT Id from inserted)
END;

CREATE TRIGGER trg_RestoranRatingiGuncelle
ON RestaurantReviews
AFTER INSERT, UPDATE
AS
BEGIN
	UPDATE Restaurants
	SET Rating = (
		SELECT AVG(Rating)
		FROM RestaurantReviews
		WHERE RestaurantId = Restaurants.Id
	)
	WHERE Id IN (SELECT RestaurantId FROM inserted)
END


CREATE TRIGGER trg_YorumSilindigindeRestoranRatingiGuncelle
ON RestaurantReviews
AFTER DELETE
AS BEGIN
	UPDATE Restaurants
	SET Rating = COALESCE((
		SELECT AVG(CAST(Rating AS FLOAT))
		FROM RestaurantReviews
		WHERE RestaurantId = Restaurants.Id
	), 5)
	WHERE Id IN (SELECT RestaurantId FROM deleted);
END;

CREATE FUNCTION YiyecekFiyatOrtalamasiniAl(@RestaurantId INT)
RETURNS DECIMAL(10,2)
AS BEGIN
	DECLARE @AveragePrice DECIMAL(10,2)
	
	SELECT @AveragePrice = AVG(Price)
	FROM Meals
	WHERE RestaurantId = @RestaurantId
	RETURN @AveragePrice
END


CREATE FUNCTION TotalCiroyuHesapla(@RestaurantId INT)
RETURNS DECIMAL(18,2)
AS BEGIN
	DECLARE @TotalRevenue DECIMAL(18,2)

	SELECT @TotalRevenue = SUM(TotalAmount)
	FROM Orders
	WHERE RestaurantId = @RestaurantId

	RETURN @TotalRevenue
END

CREATE FUNCTION MusterininTotalHarcadigiPara(@CustomerId INT)
RETURNS DECIMAL(18,2)
AS BEGIN
	DECLARE @TotalSpending DECIMAL(18,2);
	SELECT @TotalSpending = SUM(TotalAmount)
	FROM Orders
	Where CustomerId = @CustomerId

	RETURN @TotalSpending

END

CREATE VIEW vw_KuryePerformansi
AS
SELECT 
    dp.Id AS DeliveryPersonnelId,
    dp.Name AS DeliveryPersonnelName,
    COUNT(o.Id) AS TotalDeliveredOrders
FROM DeliveryPersonnel dp
LEFT JOIN Orders o ON dp.Id = o.DeliveryPersonelId
WHERE o.Status = 'Delivered'
GROUP BY dp.Id, dp.Name;


