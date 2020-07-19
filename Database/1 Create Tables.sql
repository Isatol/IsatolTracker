CREATE SCHEMA System
GO
CREATE SCHEMA Tracking
GO

CREATE TABLE System.Users(
	UserID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name varchar(50) NOT NULL,
	LastName varchar(100) NOT NULL,
	Email varchar(max) NOT NULL,
	Password varchar(max) NOT NULL
)
GO

CREATE TABLE Tracking.Company(
	CompanyID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Company VARCHAR(MAX) NOT NULL,
	Logo varchar(max) NOT NULL
)
GO
CREATE TABLE Tracking.Package(
	PackageID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TrackingNumber varchar(max) NOT NULL,
	Delivered BIT NOT NULL DEFAULT(0),
	Status varchar(max),
	EstimateDelivery DATETIME,
	UserID INT NOT NULL,
	CompanyID INT NOT NULL,
	CONSTRAINT FK_Package_User FOREIGN KEY (UserID) REFERENCES System.Users(UserID),
	CONSTRAINT FK_Package_Company FOREIGN KEY (CompanyID) REFERENCES Tracking.Company(CompanyID)
)

GO

CREATE TABLE Tracking.TrackingDetails(
	TrackingDetailsID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Date DATETIME,
	Event VARCHAR(MAX),
	Messages varchar(max),
	PackageID INT,
	CONSTRAINT FK_TrackingDetails_Package FOREIGN KEY(PackageID) REFERENCES Tracking.Package(PackageID)
)

GO

CREATE TABLE System.Notifications(
	NotificationsID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Endpoint varchar(max),
	Keys varchar(max),
	UserID INT,
	CONSTRAINT FK_Notifications_User FOREIGN KEY (UserID) REFERENCES System.Users(UserID)
)