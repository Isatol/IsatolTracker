CREATE SCHEMA Tracking
GO
CREATE SCHEMA System
GO

CREATE TABLE System.Users(
	UsersID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name varchar(max) not null,
	Email varchar(max) not null,
	Password varchar(max) not null,
	ReceiveEmails bit DEFAULT(0)
)
GO
CREATE TABLE Tracking.Company(
	CompanyID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name varchar(max),
	Logo varchar(max),
	ValidRegex varchar(max)
)
GO
CREATE TABLE Tracking.Package(
	PackageID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TrackingNumber varchar(max) not null,
	Name varchar(100),
	CompanyID int,
	UsersID INT, 
	CONSTRAINT FK_Package_Company FOREIGN KEY (CompanyID) REFERENCES Tracking.Company(CompanyID),
	CONSTRAINT FK_Package_Users FOREIGN KEY (UsersID) REFERENCES System.Users(UsersID)
)
GO
CREATE TABLE Tracking.LastPackageUpdate(
	LastPackageUpdateID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Date DATETIME,
	Event varchar(max),
	PackageID INT,
	CONSTRAINT FK_LastPackageUpdate_Package FOREIGN KEY (PackageID) REFERENCES Tracking.Package(PackageID)
)

GO

CREATE TABLE System.Notification(
	NotificationID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Endpoint varchar(max),
	Keys varchar(max),
	UsersID INT,
	CONSTRAINT FK_Notification_Users FOREIGN KEY (UsersID) REFERENCES System.Users(UsersID)
)

INSERT INTO Tracking.Company (Name, Logo) VALUES ('Estafeta', 'data:image/jpeg;base64,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'), 
('FedEx', 'data:image/jpeg;base64,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'),
('UPS', 'data:image/jpeg;base64,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')

GO

CREATE PROCEDURE Tracking.GetCompanies
AS
BEGIN
	SELECT c.CompanyID, c.Name, c.Logo, c.ValidRegex FROM Tracking.Company c
END;

GO

CREATE PROCEDURE Tracking.GetCompany
@CompanyID INT = NULL
AS

BEGIN
	SELECT c.Name, c.Logo, c.ValidRegex, c.CompanyID FROM Tracking.Company c WHERE c.CompanyID = @CompanyID
END;
GO
CREATE PROCEDURE Tracking.GetPackages
AS
BEGIN
	SELECT p.CompanyID, p.TrackingNumber, p.Name,  p.PackageID, p.UsersID FROM Tracking.Package p
END;

GO

CREATE PROCEDURE Tracking.GetLastPackageUpdate
@PackageID INT = NULL
AS
BEGIN
	SELECT l.Date, l.Event, l.LastPackageUpdateID, l.PackageID FROM Tracking.LastPackageUpdate l WHERE l.PackageID = @PackageID
END;

GO

CREATE PROCEDURE System.Login
@Email varchar(max) = null,
@Password varchar(max) = null
AS
BEGIN
	SELECT u.Email, u.Name, u.UsersID, u.Password FROM System.Users u WHERE u.Email = @Email AND u.Password = @Password
END;

GO

CREATE PROCEDURE Tracking.InsertPackage
@CompanyID INT = NULL,
@TrackingNumber varchar(max),
@UsersID INT = NULL,
@Name varchar(100) = NULL,
@LastDate DATETIME = NULL,
@LastEvent VARCHAR(MAX) = NULL
AS
BEGIN
DECLARE @PackageID INT = NULL;
	INSERT INTO Tracking.Package (CompanyID, TrackingNumber, Name, UsersID) VALUES (@CompanyID, @TrackingNumber, @Name,  @UsersID)
	SELECT @PackageID = SCOPE_IDENTITY()
	INSERT INTO Tracking.LastPackageUpdate (Date, Event, PackageID) VALUES (@LastDate, @LastEvent, @PackageID)
END;

GO

CREATE PROCEDURE Tracking.InsertLastPackageUpdate
@Date DATETIME = NULL,
@Event VARCHAR(MAX) = NULL,
@PackageID INT = NULL
AS
BEGIN
	INSERT INTO Tracking.LastPackageUpdate (Date, Event, PackageID) VALUES (@Date, @Event, @PackageID)
END;

GO

CREATE PROCEDURE System.InsertNotification
@Endpoint varchar(max) = NULL,
@Keys varchar(max) = null,
@UsersID INT = NULL
AS
BEGIN
	INSERT INTO System.Notification (Endpoint, Keys, UsersID) VALUES (@Endpoint, @Keys, @UsersID)
END;

GO

CREATE PROCEDURE System.GetNotifications
AS
BEGIN
	SELECT n.Endpoint, n.Keys, n.UsersID, n.NotificationID FROM System.Notification n
END;

GO

CREATE PROCEDURE Tracking.UpdateLastPackage
@Date DATETIME = NULL,
@Event varchar(max) = null,
@PackageID INT = NULL
AS
BEGIN
	UPDATE Tracking.LastPackageUpdate SET Date = @Date, Event = @Event WHERE PackageID = @PackageID
END;

GO

CREATE PROCEDURE System.GetUsers
AS
BEGIN
	SELECT u.Email, u.UsersID, u.Name, u.ReceiveEmails FROM System.Users u
END;

GO

CREATE PROCEDURE System.GetUserSubscription
@UserID INT = NULL
AS
BEGIN
	SELECT n.Endpoint, n.Keys, n.NotificationID, n.UsersID FROM System.Notification n WHERE n.UsersID = @UserID
END;

GO

CREATE PROCEDURE System.GetUser
@UserID INT = NULL
AS
BEGIN
	SELECT u.Email, u.Name, u.ReceiveEmails FROM System.Users u WHERE u.UsersID = @UserID	
END;

GO

CREATE PROCEDURE System.DeleteSuscription
@Endpoint varchar(max) = null,
@UserID INT = NULL
AS
BEGIN
	DELETE FROM System.Notification WHERE UsersID = @UserID AND Endpoint = @Endpoint
END;

GO

CREATE PROCEDURE System.GetUserPackages
@UserID INT = NULL
AS
BEGIN
	SELECT p.Name PackageName, p.PackageID, p.TrackingNumber, c.CompanyID, c.Name, c.Logo,
		l.Date, l.Event
	FROM Tracking.Package p JOIN Tracking.Company c ON c.CompanyID = p.CompanyID 
		LEFT JOIN Tracking.LastPackageUpdate l ON l.PackageID = p.PackageID
	WHERE p.UsersID = @UserID
END;

GO

CREATE PROCEDURE Tracking.DeletePackage
@PackageID INT = NULL
AS
BEGIN
	DELETE FROM Tracking.LastPackageUpdate WHERE PackageID = @PackageID;
	DELETE FROM Tracking.Package WHERE PackageID = @PackageID;
END;

GO

CREATE PROCEDURE System.UpdateReceiveEmails
@UserID INT = NULL,
@ReceiveEmails bit = null
AS
BEGIN
	UPDATE System.Users SET ReceiveEmails = @ReceiveEmails WHERE UsersID = @UserID
END;

GO

CREATE PROCEDURE System.AddUser
@Email varchar(max) = NULL,
@Name varchar(max) = NULL,
@Password varchar(max) = NULL
AS
BEGIN
	INSERT INTO System.Users (Email, Name, Password, ReceiveEmails) VALUES (@Email, @Name, @Password, 0)
END;

GO

CREATE PROCEDURE System.GetUserByEmail
@Email varchar(max) = NULL
AS
BEGIN
	SELECT u.Email, u.UsersID, u.Name, u.Password FROM System.Users u WHERE u.Email = @Email
END;