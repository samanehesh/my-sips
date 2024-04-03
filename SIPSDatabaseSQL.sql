-- Drop tables if they exist
DROP TABLE IF EXISTS AddIn_OrderDetail;
DROP TABLE IF EXISTS AddIn;
DROP TABLE IF EXISTS OrderDetail;
DROP TABLE IF EXISTS Item;
DROP TABLE IF EXISTS ItemSize;
DROP TABLE IF EXISTS ItemType;
DROP TABLE IF EXISTS ImageStores;
DROP TABLE IF EXISTS Ice;
DROP TABLE IF EXISTS Sweetness;
DROP TABLE IF EXISTS [Transaction];
DROP TABLE IF EXISTS [OrderStatus];
DROP TABLE IF EXISTS Rating;
DROP TABLE IF EXISTS [Store];
DROP TABLE IF EXISTS Contact;
DROP TABLE IF EXISTS MilkChoice;
DROP TABLE IF EXISTS PaymentNotification;

CREATE TABLE PaymentNotification (
    PaymentID NVARCHAR(30) PRIMARY KEY,
    Amount NVARCHAR(30),
    CurrencyCode NVARCHAR(10),
    CurrencySymbol NVARCHAR(10),
    PayerId NVARCHAR(30),
    PayerFullName NVARCHAR(50),
    CaptureId NVARCHAR(30)
)

-- Create ItemType table
CREATE TABLE ItemType (
    itemTypeID INTEGER PRIMARY KEY IDENTITY(1,1),
    itemTypeName VARCHAR(50) NOT NULL
);

-- Create ImageStores table
CREATE TABLE ImageStores( 
    ImageID    INTEGER IDENTITY(1,1) NOT NULL,
    [FileName] VARCHAR(20)       NOT NULL,
    [Image]    VARBINARY(MAX)    NOT NULL, 
    CONSTRAINT PK_ImageStores PRIMARY KEY CLUSTERED ( ImageId ASC )
);

-- Create ItemSize table
CREATE TABLE ItemSize (
    sizeID INTEGER PRIMARY KEY IDENTITY(1,1),
    sizeName VARCHAR(30) NOT NULL,
    priceModifier DECIMAL(10, 2) NOT NULL
);

-- Create Sweetness table
CREATE TABLE Sweetness (
    sweetnessID INTEGER PRIMARY KEY IDENTITY(1,1),
    sweetnessPercent VARCHAR(30) NOT NULL
);

-- Create Ice table
CREATE TABLE Ice (
    iceID INTEGER PRIMARY KEY IDENTITY(1,1),
    icePercent VARCHAR(30) NOT NULL
);

-- Create AddIn table
CREATE TABLE AddIn (
    addInID INTEGER PRIMARY KEY IDENTITY(1,1),
    addInName VARCHAR(30) NOT NULL,
    priceModifier DECIMAL(10, 2) NOT NULL,
);

-- Create Store table
CREATE TABLE [Store] (
    storeID INTEGER PRIMARY KEY IDENTITY(1,1),
    storeHours VARCHAR(255) NOT NULL
);

-- Create OrderStatus table
CREATE TABLE [OrderStatus] (
    statusID INTEGER PRIMARY KEY IDENTITY(1,1),
    isCompleted BIT NOT NULL
);

-- Create Contact table
CREATE TABLE Contact (
    userID INTEGER PRIMARY KEY IDENTITY(1,1),
    firstName VARCHAR(30) NOT NULL,
    lastName VARCHAR(30),
    phoneNumber VARCHAR(20) NOT NULL,
    email VARCHAR(50) NOT NULL,
    unit INTEGER,
    street VARCHAR(50),
    city VARCHAR(50),
    province VARCHAR(20),
    postalCode VARCHAR(10),
    birthDate DATE,
    isDrinkRedeemed BIT NOT NULL
);

-- Create Transaction table
CREATE TABLE [Transaction] (
    transactionID VARCHAR(30) PRIMARY KEY,
    dateOrdered DATE NOT NULL,
    storeID INTEGER NOT NULL,
    userID INTEGER NOT NULL,
    statusID INTEGER NOT NULL, -- Assuming this is the foreign key to OrderStatus table
    FOREIGN KEY (storeID) REFERENCES Store(storeID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (userID) REFERENCES Contact(userID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (statusID) REFERENCES OrderStatus(statusID) ON UPDATE CASCADE ON DELETE CASCADE
);

-- Create Rating table
CREATE TABLE Rating (
    ratingID INTEGER PRIMARY KEY IDENTITY(1,1),
    rating VARCHAR(5) NOT NULL,
    date DATE NOT NULL,
    comment VARCHAR(255) NOT NULL,
    storeID INTEGER NOT NULL,
    userID INTEGER NOT NULL,
    FOREIGN KEY (storeID) REFERENCES Store(storeID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (userID) REFERENCES Contact(userID) ON UPDATE CASCADE ON DELETE CASCADE
);

-- Create Item table
CREATE TABLE Item (
    itemID INTEGER PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    basePrice DECIMAL(10, 2) NOT NULL,
    inventory INTEGER NOT NULL,
    itemTypeID INTEGER,
    imageID INTEGER,
    hasMilk BIT NOT NULL
    FOREIGN KEY (itemTypeID) REFERENCES ItemType(itemTypeID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (imageID) REFERENCES ImageStores(ImageID) ON UPDATE CASCADE ON DELETE CASCADE
);

-- Create MilkChoice table
CREATE TABLE MilkChoice (
    milkChoiceID INTEGER PRIMARY KEY IDENTITY(1,1),
    milkType VARCHAR(50) NOT NULL,
    priceModifier DECIMAL(10, 2) NOT NULL
);

-- Create OrderDetail table
CREATE TABLE OrderDetail (
    orderDetailID INT IDENTITY(1,1) PRIMARY KEY, -- Use INT for auto-incrementing ID
    price DECIMAL(10, 2) NOT NULL,
    quantity INTEGER NOT NULL,
    isBirthdayDrink BIT NOT NULL,
    promoValue DECIMAL(10, 2),
    itemID INTEGER NOT NULL,
    transactionID VARCHAR(30) NOT NULL,
    sizeID INTEGER NOT NULL,
    sweetnessID INTEGER,
    iceID INTEGER,
    milkChoiceID INTEGER, -- New column for MilkChoice
    FOREIGN KEY (itemID) REFERENCES Item(itemID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (transactionID) REFERENCES [Transaction](transactionID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (sizeID) REFERENCES ItemSize(sizeID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (sweetnessID) REFERENCES Sweetness(sweetnessID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (iceID) REFERENCES Ice(iceID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (milkChoiceID) REFERENCES MilkChoice(milkChoiceID) ON UPDATE CASCADE ON DELETE CASCADE -- New foreign key
);

-- Create AddIn_OrderDetail table
CREATE TABLE AddIn_OrderDetail (
    addInID INTEGER NOT NULL,
    orderDetailID INT NOT NULL,
    quantity INTEGER NOT NULL,
    PRIMARY KEY (addInID, orderDetailID),
    FOREIGN KEY (addInID) REFERENCES AddIn(addInID) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (orderDetailID) REFERENCES OrderDetail(orderDetailID) ON UPDATE CASCADE ON DELETE CASCADE
);

-- Pre-populate Sweetness table
INSERT INTO Sweetness (sweetnessPercent) VALUES ('0% Sweet');
INSERT INTO Sweetness (sweetnessPercent) VALUES ('25% Sweet');
INSERT INTO Sweetness (sweetnessPercent) VALUES ('50% Sweet');
INSERT INTO Sweetness (sweetnessPercent) VALUES ('75% Sweet');
INSERT INTO Sweetness (sweetnessPercent) VALUES ('100% Sweet');

-- Pre-populate Ice table
INSERT INTO Ice (icePercent) VALUES ('No Ice');
INSERT INTO Ice (icePercent) VALUES ('Less Ice');
INSERT INTO Ice (icePercent) VALUES ('Regular Ice');
INSERT INTO Ice (icePercent) VALUES ('Extra Ice');

-- Pre-populate MilkChoice table
INSERT INTO MilkChoice (milkType, priceModifier) VALUES ('Regular', 0.00);
INSERT INTO MilkChoice (milkType, priceModifier) VALUES ('Soy', 0.80);
INSERT INTO MilkChoice (milkType, priceModifier) VALUES ('Almond', 0.80);
INSERT INTO MilkChoice (milkType, priceModifier) VALUES ('Oat', 0.80);
INSERT INTO MilkChoice (milkType, priceModifier) VALUES ('No Milk', 0.00);

-- Pre-populate AddIn table
INSERT INTO AddIn (addInName, priceModifier) VALUES ('Pearls', 1.25);
INSERT INTO AddIn (addInName, priceModifier) VALUES ('Sago', 1.25);
INSERT INTO AddIn (addInName, priceModifier) VALUES ('Lychee Jelly', 1.25);
INSERT INTO AddIn (addInName, priceModifier) VALUES ('Pudding', 1.25);

-- Insert statements for ItemType table
INSERT INTO ItemType (itemTypeName) VALUES ('Milk Tea');
INSERT INTO ItemType (itemTypeName) VALUES ('Fruit Tea');
INSERT INTO ItemType (itemTypeName) VALUES ('Slush');

-- Milk Teas
INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Matcha Milk Tea', 'A delightful blend of matcha and creamy milk.', 4.99, 100, 1, 1);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Taro Milk Tea', 'Experience the unique flavor of taro in a refreshing milk tea.', 4.99, 100, 1, 1);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Brown Sugar Milk Tea', 'Indulge in the rich taste of brown sugar infused in milk tea.', 5.49, 100, 1, 1);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Sips Milk Tea', 'Our classic and flavorful Sips Milk Tea.', 3.99, 100, 1, 1);

-- Fruit Teas
INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('PassionFruit Tea', 'A tropical burst of passion fruit in a refreshing tea.', 5.69, 100, 2, 0);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Peach Kiwi Tea', 'Experience the perfect harmony of peach and kiwi in tea.', 5.99, 100, 2, 0);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Mango Tea', 'Enjoy the sweetness of mango in our delightful tea blend.', 5.99, 100, 2, 0);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Wintermelon Tea', 'Refresh yourself with the cooling taste of wintermelon tea.', 6.29, 100, 2, 0);

-- Slushes
INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Watermelon Raspberry Slush', 'Savor the unique and sweet taste of watermelon and raspberry in a slush.', 6.49, 100, 3, 0);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Taro Slush', 'Experience the rich and creamy taro in a delightful slush.', 6.29, 100, 3, 1);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Strawberry Slush', 'Enjoy the freshness of strawberry in a chilled slush drink.', 5.99, 100, 3, 0);

INSERT INTO Item (name, description, basePrice, inventory, itemTypeID, hasMilk)
VALUES ('Coffee Slush', 'A delightful mix of coffee flavor in a frosty slush.', 5.99, 100, 3, 1);


-- OrderStatus
INSERT INTO [OrderStatus] (isCompleted) VALUES (0);
INSERT INTO [OrderStatus] (isCompleted) VALUES (1);

INSERT INTO Contact (firstName, lastName, phoneNumber, email, unit, street, city, province, postalCode, birthDate, isDrinkRedeemed)
VALUES ('everest', 'shi', 'YOUR_PHONE_NUMBER', 'shi.everest@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 0);
INSERT INTO Contact (firstName, lastName, phoneNumber, email, unit, street, city, province, postalCode, birthDate, isDrinkRedeemed)
VALUES ('eunice', 'ssd', 'YOUR_PHONE_NUMBER', 'eunicezh10@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 0);
INSERT INTO Contact (firstName, lastName, phoneNumber, email, unit, street, city, province, postalCode, birthDate, isDrinkRedeemed)
VALUES ('sydnee', 'ssd', 'YOUR_PHONE_NUMBER', 's.snowball@hotmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 0);
INSERT INTO Contact (firstName, lastName, phoneNumber, email, unit, street, city, province, postalCode, birthDate, isDrinkRedeemed)
VALUES ('pam', 'ssd', 'YOUR_PHONE_NUMBER', 'pampragides@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 0);
INSERT INTO Contact (firstName, lastName, phoneNumber, email, unit, street, city, province, postalCode, birthDate, isDrinkRedeemed)
VALUES ('samaneh', 'ssd', 'YOUR_PHONE_NUMBER', 'sama.heshmatzadeh@gmail.com', NULL, NULL, NULL, NULL, NULL, NULL, 0);



