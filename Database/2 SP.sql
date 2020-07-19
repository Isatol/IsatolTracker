CREATE PROCEDURE System.Login @Email    VARCHAR(MAX) = NULL, 
                              @Password VARCHAR(MAX) = NULL
AS
    BEGIN
        SELECT u.Name, 
               u.Email, 
               u.LastName, 
               u.UserID
        FROM System.Users u
        WHERE u.Email = @Email
              AND u.Password = @Password;
    END;
GO
CREATE PROCEDURE System.AddNotificationSubscription @Endpoint VARCHAR(MAX) = NULL, 
                                                    @Keys     VARCHAR(MAX) = NULL, 
                                                    @UserID   INT          = NULL
AS
    BEGIN
        INSERT INTO System.Notifications
        (Endpoint, 
         Keys, 
         UserID
        )
        VALUES
        (@Endpoint, 
         @Keys, 
         @UserID
        );
    END;
GO
CREATE PROCEDURE System.GetUserNotificationSubscription @UserID INT = NULL
AS
    BEGIN
        SELECT n.Endpoint, 
               n.Keys, 
               n.NotificationsID
        FROM System.Notifications n
        WHERE n.UserID = @UserID;
    END;
GO
CREATE PROCEDURE Tracking.GetUserPackages @UserID INT = NULL
AS
    BEGIN
        SELECT p.PackageID, 
               p.Delivered, 
               p.EstimateDelivery, 
               p.STATUS, 
               p.TrackingNumber, 
               c.Company, 
               c.CompanyID
        FROM Tracking.Package p
             JOIN Tracking.Company c ON p.CompanyID = c.CompanyID
        WHERE p.UserID = @UserID;
    END;
GO
CREATE PROCEDURE Tracking.GetPackages
AS
    BEGIN
        SELECT p.PackageID, 
               p.Delivered, 
               p.EstimateDelivery, 
               p.STATUS, 
               p.TrackingNumber, 
               c.Company, 
               c.CompanyID,
			   p.UserID
        FROM Tracking.Package p
             JOIN Tracking.Company c ON p.CompanyID = c.CompanyID;
    END;
GO
CREATE PROCEDURE Tracking.AddPackage @TrackingNumber   VARCHAR(MAX) = NULL, 
                                     @Delivered        BIT          = 0, 
                                     @EstimateDelivery DATETIME     = NULL, 
                                     @Status           VARCHAR(MAX) = NULL, 
                                     @UserID           INT          = NULL
AS
    BEGIN
        INSERT INTO Tracking.Package
        (TrackingNumber, 
         Delivered, 
         EstimateDelivery, 
         STATUS, 
         UserID
        )
        VALUES
        (@TrackingNumber, 
         @Delivered, 
         @EstimateDelivery, 
         @Status, 
         @UserID
        );
    END;
GO
CREATE PROCEDURE Tracking.GetPackageDetails @PackageID INT = NULL
AS
    BEGIN
        SELECT td.TrackingDetailsID, 
               td.Date, 
               td.Event, 
               td.Messages
        FROM Tracking.TrackingDetails td
        WHERE td.PackageID = @PackageID;
    END;
GO
CREATE PROCEDURE Tracking.AddPackageDetails @Date      DATETIME     = NULL, 
                                            @Event     VARCHAR(MAX) = NULL, 
                                            @Messages  VARCHAR(MAX) = NULL, 
                                            @PackageID INT          = NULL
AS
    BEGIN
        INSERT INTO Tracking.TrackingDetails
        (Date, 
         Event, 
         Messages, 
         PackageID
        )
        VALUES
        (@Date, 
         @Event, 
         @Messages, 
         @PackageID
        );
    END;