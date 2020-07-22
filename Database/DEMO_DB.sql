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

INSERT INTO Tracking.Company (Name, Logo) VALUES ('Estafeta', 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4KPCEtLSBHZW5lcmF0b3I6IEFkb2JlIElsbHVzdHJhdG9yIDE5LjIuMSwgU1ZHIEV4cG9ydCBQbHVnLUluIC4gU1ZHIFZlcnNpb246IDYuMDAgQnVpbGQgMCkgIC0tPgo8c3ZnIHZlcnNpb249IjEuMSIgaWQ9IkNhcGFfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiCgkgd2lkdGg9IjIzOC41cHgiIGhlaWdodD0iNDlweCIgdmlld0JveD0iMCAwIDIzOC41IDQ5IiBlbmFibGUtYmFja2dyb3VuZD0ibmV3IDAgMCAyMzguNSA0OSIgeG1sOnNwYWNlPSJwcmVzZXJ2ZSI+CjxnPgoJPGc+CgkJPHBhdGggZmlsbD0iI0MwMTgxOCIgZD0iTTU0LjMyNCwyMi43MjVsNi45MDYsNS44OWMxLjMwMiwxLjIzNCwyLjEwOSwyLjk5NywyLjEwOSw0Ljk0MWMwLDMuNzIzLTIuOTgxLDYuNzQzLTYuNjY2LDYuNzQzCgkJCWwtNC43MjQsMC4wMTZIMzcuNDYxbDEuNDc4LTYuNDY4bDEwLjkwMSwwLjAxOWMxLjE4MSwwLDIuMTM4LTAuOTY1LDIuMTM4LTIuMTU2YzAtMC43MzgtMC4zNjItMS4zODgtMC45MjQtMS43NzlsLTYuMDg3LTQuNDg2CgkJCWMtMS42Mi0xLjIyOS0yLjY2OC0zLjE5OC0yLjY2OC01LjQwNGMwLTMuNzM1LDIuOTg0LTYuNzUzLDYuNjYzLTYuNzUzbDE1Ljc1NS0wLjAxMWwtMS4yOTEsNS42OTdoLTcuNjM5CgkJCWMtMS4xNzcsMC0yLjEzMywwLjk2My0yLjEzMywyLjE1MmMwLDAuNjMxLDAuMjYsMS4xOTgsMC42ODMsMS41OTNMNTQuMzI0LDIyLjcyNXoiLz4KCQk8cGF0aCBmaWxsPSIjQzAxODE4IiBkPSJNODAuMzAxLDM0LjY4N2gwLjAyOGMtMS4xNzksMC0yLjE1Mi0wLjk2My0yLjEzMy0yLjE1OGMwLTAuMjUyLDAuMDYtMC40NDYsMC4xMTgtMC42MzdsMi45Mi0xMi45NThoNC44NjgKCQkJbDEuMjkyLTUuNjU5aC00Ljg2N2wxLjYzNC03LjA3NGgtOS4zMzFsLTEuNjIzLDcuMDc1aC01LjA5NGwtMS4zMDIsNS42NTdoNS4xMDhsLTMuNzY0LDE2LjQ3MwoJCQljLTAuMDYsMC4yODUtMC4wODgsMC41MzItMC4wODgsMC44NzhjMCwyLjIyNywxLjc5LDQuMDQ4LDQuMDA0LDQuMDQ4bDAsMGw4LjA0My0wLjAzNWwxLjI5Ny01LjYzNwoJCQlDODEuNDE0LDM0LjY2Miw4MC43MTMsMzQuNjg3LDgwLjMwMSwzNC42ODciLz4KCQk8cGF0aCBmaWxsPSIjQzAxODE4IiBkPSJNMTgzLjA2MiwzNC42ODdoMC4wMjFjLTEuMTc5LDAtMi4xNDctMC45NjMtMi4xMjctMi4xNThjMC0wLjI1MiwwLjA1NS0wLjQ0NiwwLjEwOS0wLjYzN2wyLjkyNi0xMi45NTgKCQkJaDQuODY2bDEuMjkxLTUuNjU5aC00Ljg1OWwxLjYxNS03LjA3NGgtOS4zMDlsLTEuNjM5LDcuMDc1aC01LjA4MmwtMS4zMDIsNS42NTdoNS4xMDlsLTMuNzY3LDE2LjQ3MwoJCQljLTAuMDYzLDAuMjg1LTAuMDk4LDAuNTMyLTAuMDk4LDAuODc4YzAuMDA1LDIuMjI3LDEuNzkxLDQuMDQ4LDQuMDAxLDQuMDQ4aDAuMDA3bDguMDUzLTAuMDM1bDEuMjg4LTUuNjM3CgkJCUMxODQuMTY0LDM0LjY2MiwxODMuNDgsMzQuNjg3LDE4My4wNjIsMzQuNjg3Ii8+CgkJPHBhdGggZmlsbD0iI0MwMTgxOCIgZD0iTTExNi4yNjksNDAuMjg1YzAuNjExLTAuMjYsMS4wMzUtMC43OTQsMS4xOTMtMS40NTlsMC4wNTItMC4yMTZsLTAuMDI3LDAuMDVsNC41MjItMTkuNzM3aC0zLjczMgoJCQlsMS4yOC01LjY1MmgzLjczN2wwLjQxNi0xLjgzOWwwLjA5NC0wLjM3M2MwLjgyMi0yLjc5MiwzLjM3NS00LjgyOCw2LjM5OS00LjgyOGwwLjAxNC0wLjAxOWg4Ljg1NGwtMS4wMzMsNC41NDFsLTMuMTQ4LTAuMDA5CgkJCWMtMS4wMTIsMC0xLjg0LDAuNzEtMi4wNzYsMS42NTR2LTAuMDM2bC0wLjIxNywwLjkwOGg0Ljg2M2wtMS4yNzQsNS42NzRoLTQuODc3bC00LjAyMiwxNy43MDEKCQkJYy0wLjQyNiwyLjA4My0yLjI0NCwzLjY0OC00LjQzNCwzLjY0OEwxMTYuMjY5LDQwLjI4NXoiLz4KCQk8cGF0aCBmaWxsPSIjQzAxODE4IiBkPSJNOTguNjc3LDMzLjk5NWwtMC4wNDIsMC4wMmMtMC41MzgsMC4zNjEtMS4xODgsMC41NjUtMS44OCwwLjU2NWMtMS45MTcsMC0zLjQ2OS0xLjU2Ni0zLjQ2OS0zLjUwNwoJCQljMC0xLjkzOSwxLjU1Mi0zLjUxNSwzLjQ2OS0zLjUxNWw2LjU3NSwwLjAyNEMxMDIuNzc3LDMwLjM0NSwxMDEuMDQsMzIuNjc2LDk4LjY3NywzMy45OTUgTTExNS4wMzYsMTYuNzc4CgkJCWMwLTEuOTM5LTEuNTU3LTMuNTAzLTMuNDY4LTMuNTAzSDkwLjc5NWwtMS4yNjksNS42MjFoMTQuMzQyYzAuNTk4LDAsMS4wODMsMC40NzUsMS4wODMsMS4wNzJjMCwwLjA4Mi0wLjAwNSwwLjE1OS0wLjAyNSwwLjIzOAoJCQloLTAuMDEzbC0wLjU3MSwyLjQ4NEg5NC4yODhjLTUuMTUyLDAtOS4zMzEsNC4yMzYtOS4zMzEsOS40NTJjMCwwLjA3NywwLDAuMTYyLDAuMDA1LDAuMjQxYzAsMCwwLjAwMywwLjI2OCwwLjAwOSwwLjQ0MQoJCQljMC4xNDIsNC44NjYsNC41MSw4LjI0LDkuMjY4LDguMjRjMC41OTgsMCwxLjA4My0wLjA0NCwxLjc1Ny0wLjE2YzEuOTEyLTAuMzQyLDMuMzI2LTAuODg4LDQuNjI2LTEuOTEKCQkJYzAuNjMxLDAuNzk4LDEuNjIxLDEuMjkxLDIuNzA3LDEuMjkxaDguNzc5Yy0wLjk1Ni0wLjYyMi0xLjU4Mi0xLjczLTEuNTgyLTIuOTY2YzAtMC4yOTYsMC4wMzEtMC41ODQsMC4wOTYtMC44NjRsNC4zMjctMTguOTMzCgkJCXYwLjAyN0MxMTUuMDA1LDE3LjMwMiwxMTUuMDM2LDE3LjA0MiwxMTUuMDM2LDE2Ljc3OCIvPgoJCTxwYXRoIGZpbGw9IiNDMDE4MTgiIGQ9Ik0yOS41NDgsMjAuMjE0bC0wLjU4NCwyLjUxNUgyMC44MWwwLjQ1Ny0xLjkxOWMwLjEzNS0xLjA1MiwxLjAxMy0xLjg2MSwyLjA4NC0xLjg4MWw1LjE1MiwwLjAwNgoJCQljMC41ODksMCwxLjA2MiwwLjQ4LDEuMDYyLDEuMDgzQzI5LjU2NSwyMC4wODUsMjkuNTY1LDIwLjE0NywyOS41NDgsMjAuMjE0IE0zNi4xODcsMTMuMjc3aC0wLjA0MUgxNy4zNmwtMC4wNjktMC4wMzMKCQkJYy0yLjExNywwLTMuOTA3LDEuNDc1LTQuMzkzLDMuNDc2TDguNDMsMzYuMDczYy0wLjA1MywwLjI0MS0wLjA2OSwwLjQ2OS0wLjA2OSwwLjcyNGMwLDEuOTQxLDEuNTU4LDMuNTEyLDMuNDc0LDMuNTEyaDIyLjUyMgoJCQlsMS40NzItNi40OTFIMjAuOTQ1bDAuMDMsMC4wMTFjLTEuMTg0LDAtMi4xNDQtMC45NjYtMi4xNDQtMi4xNTdjMC0wLjE4NCwwLjAzNS0wLjM2NCwwLjA3MS0wLjUzM2wtMC4wMTEsMC4wMTlsMC43MjQtMy4zMjUKCQkJaDE3LjYwM2wyLjM4Ni0xMC40MDNjMC4wMzMtMC4yMTQsMC4wNS0wLjQzLDAuMDUtMC42NDlDMzkuNjU1LDE0Ljg0OSwzOC4xMDYsMTMuMjc3LDM2LjE4NywxMy4yNzciLz4KCQk8cGF0aCBmaWxsPSIjQzAxODE4IiBkPSJNMjAxLjQzLDMzLjk5OGwtMC4wMzYsMC4wMTdjLTAuNTQ4LDAuMzYxLTEuMTkyLDAuNTY1LTEuODksMC41NjVjLTEuOTEzLDAtMy40NTUtMS41NjYtMy40NTUtMy41MDcKCQkJYzAtMS45NDIsMS41NDMtMy41MTUsMy40NTUtMy41MTVsNi41NzksMC4wMkMyMDUuNTMyLDMwLjM0MiwyMDMuNzk0LDMyLjY3MSwyMDEuNDMsMzMuOTk4IE0yMTQuMzMsMTMuMjcxaC0yMC43ODNsLTEuMjczLDUuNjI2CgkJCWgxNC4zNTNjMC41OTgsMCwxLjA4LDAuNDc3LDEuMDgsMS4wNzJjMCwwLjA4NC0wLjAwNiwwLjE1OS0wLjAyMywwLjI0MmwtMC4wMTYtMC4wMDVsLTAuNTYsMi40ODRoLTEwLjA1OAoJCQljLTUuMTQ5LDAtOS4zMyw0LjIzOS05LjMzLDkuNDUydjAuMjQzYzAsMCwwLjAxLDAuMjY5LDAuMDEsMC40NDJjMC4xMzksNC44NjMsNC41MTMsOC4yNCw5LjI2Miw4LjI0CgkJCWMwLjYwMSwwLDEuMDg4LTAuMDQ3LDEuNzYzLTAuMTYzYzEuOTE1LTAuMzQyLDMuMzE3LTAuODg4LDQuNjMtMS45MWMwLjYzMywwLjgwMSwxLjYyMSwxLjI5OSwyLjcwNCwxLjI5OWg4Ljc4MgoJCQljLTAuOTYyLTAuNjMtMS41OTItMS43MzgtMS41OTItMi45NzRjMC0wLjI5MywwLjAzNS0wLjU4NCwwLjEwNS0wLjg2NGw0LjMyMy0xOC45MzJ2MC4wMjVjMC4wNTUtMC4yNDcsMC4wOS0wLjUwNSwwLjA5LTAuNzcxCgkJCUMyMTcuNzk0LDE0LjgzNSwyMTYuMjM3LDEzLjI3MSwyMTQuMzMsMTMuMjcxIi8+CgkJPHBhdGggZmlsbD0iI0MwMTgxOCIgZD0iTTE1Ni40NzcsMjAuMjM0bC0wLjU3NSwyLjUxOWgtOC4xNTVsMC40NTgtMS45MTljMC4xMjQtMS4wNDcsMS4wMDktMS44NjcsMi4wNzctMS44ODRsNS4xNSwwLjAwMwoJCQljMC41ODksMCwxLjA2NywwLjQ4NywxLjA2NywxLjA4M0MxNTYuNDk5LDIwLjEwNCwxNTYuNDkzLDIwLjE3LDE1Ni40NzcsMjAuMjM0IE0xNjMuMTI1LDEzLjI5MWgtMC4wNDZoLTE4Ljc4NGwtMC4wNjYtMC4wMwoJCQljLTIuMTMxLDAtMy45MDgsMS40NzgtNC40MDUsMy40NzJsLTQuNDY0LDE5LjM2M2MtMC4wNTUsMC4yNDQtMC4wNjYsMC40NjYtMC4wNjYsMC43MThjMCwxLjk0NCwxLjU1NCwzLjUxNiwzLjQ2NSwzLjUxNmgyMi41MjcKCQkJbDEuNDc4LTYuNDk1aC0xNC44ODlsMC4wMjgsMC4wMTFjLTEuMTg0LDAtMi4xMzMtMC45NjItMi4xMzMtMi4xNTJjMC0wLjE4OSwwLjAyLTAuMzY4LDAuMDY1LTAuNTM4bC0wLjAyNSwwLjAyM2wwLjczOC0zLjMzNgoJCQloMTcuNjA5bDIuMzcyLTEwLjQwNmMwLjA0MS0wLjIwNSwwLjA2Mi0wLjQxNiwwLjA2Mi0wLjYzNUMxNjYuNTkxLDE0Ljg2MywxNjUuMDQyLDEzLjI5MSwxNjMuMTI1LDEzLjI5MSIvPgoJPC9nPgoJPGc+CgkJPHBhdGggZmlsbD0iI0MwMTgxOCIgZD0iTTIyNC40MzUsMTcuNTI3aDAuNjc0bDEuMTAyLDEuODM4aDAuODAxbC0xLjIwNi0xLjg5MmMwLjY1My0wLjA2OSwxLjExNy0wLjM5MywxLjExNy0xLjE2OAoJCQljMC0wLjg3Ny0wLjUxOC0xLjIzNy0xLjU2Ni0xLjIzN2gtMS42MnY0LjI5N2gwLjY5N1YxNy41Mjd6IE0yMjQuNDM1LDE2LjkyN3YtMS4yNTZoMC44NTZjMC40MjIsMCwwLjkzMywwLjA2MywwLjkzMywwLjU3OQoJCQljMCwwLjYyNS0wLjQ3MiwwLjY3Ny0wLjk5MiwwLjY3N0gyMjQuNDM1eiBNMjI5LDE3LjE0M2MwLTIuMzMxLTEuNzYtMy44ODItMy43ODktMy44ODJjLTIuMDY0LDAtMy44MTcsMS41NS0zLjgxNywzLjg4MgoJCQljMCwyLjIxLDEuNTg1LDMuODgsMy44MTcsMy44OEMyMjcuMjQsMjEuMDIzLDIyOSwxOS40NzksMjI5LDE3LjE0MyBNMjI4LjE3MiwxNy4xNDNjMCwxLjg5Ny0xLjMxMywzLjIwMy0yLjk2MSwzLjIwMwoJCQljLTEuODUxLDAtMi45OTItMS40NC0yLjk5Mi0zLjIwM2MwLTEuODgzLDEuMzAyLTMuMjAzLDIuOTkyLTMuMjAzQzIyNi44NTksMTMuOTM5LDIyOC4xNzIsMTUuMjYsMjI4LjE3MiwxNy4xNDMiLz4KCTwvZz4KPC9nPgo8L3N2Zz4K'), 
('FedEx', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAALAAAAAyCAYAAAD8z1GNAAAACXBIWXMAABcSAAAXEgFnn9JSAAA6LGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDggNzkuMTY0MDUwLCAyMDE5LzEwLzAxLTE4OjAzOjE2ICAgICAgICAiPgogPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgeG1sbnM6ZGFtPSJodHRwOi8vd3d3LmRheS5jb20vZGFtLzEuMCIKICAgIHhtbG5zOnRpZmY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vdGlmZi8xLjAvIgogICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iCiAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iCiAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgIGRhbTpQaHlzaWNhbGhlaWdodGluaW5jaGVzPSIwLjMzMzMwNTc3NjExOTIzMjIiCiAgIGRhbTpQaHlzaWNhbHdpZHRoaW5pbmNoZXM9IjEuMTczMjM2MzcwMDg2NjciCiAgIGRhbTpGaWxlZm9ybWF0PSJQTkciCiAgIGRhbTpQcm9ncmVzc2l2ZT0ibm8iCiAgIGRhbTpleHRyYWN0ZWQ9IjIwMjAtMDItMDFUMDQ6NDY6NTEuMjA1WiIKICAgZGFtOkJpdHNwZXJwaXhlbD0iMzIiCiAgIGRhbTpNSU1FdHlwZT0iaW1hZ2UvcG5nIgogICBkYW06UGh5c2ljYWx3aWR0aGluZHBpPSIxNTAiCiAgIGRhbTpQaHlzaWNhbGhlaWdodGluZHBpPSIxNTAiCiAgIGRhbTpOdW1iZXJvZmltYWdlcz0iMSIKICAgZGFtOk51bWJlcm9mdGV4dHVhbGNvbW1lbnRzPSIwIgogICBkYW06c2hhMT0iMmQ3MzE3Zjk1MzU1MzJhYzRkOGI1NzlhMDgzZjU2Yjg5NDI2ZDFiNSIKICAgZGFtOnNpemU9IjE3OTY0IgogICB0aWZmOkltYWdlTGVuZ3RoPSI1MCIKICAgdGlmZjpZUmVzb2x1dGlvbj0iMTUwMDAwMC8xMDAwMCIKICAgdGlmZjpPcmllbnRhdGlvbj0iMSIKICAgdGlmZjpSZXNvbHV0aW9uVW5pdD0iMiIKICAgdGlmZjpYUmVzb2x1dGlvbj0iMTUwMDAwMC8xMDAwMCIKICAgdGlmZjpJbWFnZVdpZHRoPSIxNzYiCiAgIGV4aWY6Q29sb3JTcGFjZT0iNjU1MzUiCiAgIGV4aWY6UGl4ZWxZRGltZW5zaW9uPSI1MCIKICAgZXhpZjpQaXhlbFhEaW1lbnNpb249IjE3NiIKICAgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxNS41IChNYWNpbnRvc2gpIgogICB4bXA6TWV0YWRhdGFEYXRlPSIyMDE3LTA4LTIxVDE2OjI1OjA5LjAwMC0wNDowMCIKICAgeG1wOk1vZGlmeURhdGU9IjIwMTctMDgtMjFUMTY6MjU6MDkuMDAwLTA0OjAwIgogICB4bXA6Q3JlYXRlRGF0ZT0iMjAxNy0wOC0yMVQxNjoyMDo0OC4wMDAtMDQ6MDAiCiAgIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIgogICBkYzptb2RpZmllZD0iMjAxNy0xMC0wN1QxNDozOTozMy41NzZaIgogICB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6MzExMzc0NzUtYzczZC0xMTdhLWEwZDItY2E4MGIwMTc3NzBlIgogICB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6NTUzODk3MjEtNjVmMy00MWFlLTg3YTItNWM1YWU4MTI0YzRlIgogICB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjAxZTYzYTVkLWMwZTEtNDVhMS05N2UzLWViYWZhZjFlMmYyMCIKICAgcGhvdG9zaG9wOkNvbG9yTW9kZT0iMyI+CiAgIDx4bXBNTTpIaXN0b3J5PgogICAgPHJkZjpTZXE+CiAgICAgPHJkZjpsaQogICAgICBzdEV2dDphY3Rpb249InNhdmVkIgogICAgICBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxNS41IChNYWNpbnRvc2gpIgogICAgICBzdEV2dDp3aGVuPSIyMDE3LTA4LTIxVDE2OjI0OjQzLjAwMC0wNDowMCIKICAgICAgc3RFdnQ6Y2hhbmdlZD0iLyIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo1NTM4OTcyMS02NWYzLTQxYWUtODdhMi01YzVhZTgxMjRjNGUiLz4KICAgICA8cmRmOmxpCiAgICAgIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1LjUgKE1hY2ludG9zaCkiCiAgICAgIHN0RXZ0OndoZW49IjIwMTctMDgtMjFUMTY6MjU6MDkuMDAwLTA0OjAwIgogICAgICBzdEV2dDpjaGFuZ2VkPSIvIgogICAgICBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjAxZTYzYTVkLWMwZTEtNDVhMS05N2UzLWViYWZhZjFlMmYyMCIvPgogICAgPC9yZGY6U2VxPgogICA8L3htcE1NOkhpc3Rvcnk+CiAgPC9yZGY6RGVzY3JpcHRpb24+CiA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgo8P3hwYWNrZXQgZW5kPSJyIj8+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InIiPz52R9PkAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAAt6SURBVHja7J17lFVVHcc/d+6FeTjDS1B5IwiywEiBpCCiBw9x2VMrjYhyWYppT+1hgYWaFGZWVGSpWGRiZaGLpSHCiiBhmfEIiQSnSUBeAzPIMO+5pz/2964Zprn37n3mnjN3Zu53rVl3zczZ57HPd//27/Hd+0Y8zysF+hAONgGLgV10HUwGfgkMS3tk6QvwyAI4ug8igd7TKWAqcJgujhgwCMgP6XoDgB5drA97AhcAfbPons4BonQD5AHVIV6vFoh3sT6MAzVZdk/VgNddCJxDDjkC55BDjsA55JAjcA45AueQQ47AOeTQ9QjcbfKTHYxe3cU4xXy0OYKppLkWJCLAS0Bljl9WqAb2ACcd+zoCnCb7ctNZQ+CtwM1AgQ9rf0YvJIf0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'),
('UPS', 'data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz4NCjwhLS0gR2VuZXJhdG9yOiBBZG9iZSBJbGx1c3RyYXRvciAyMy4xLjAsIFNWRyBFeHBvcnQgUGx1Zy1JbiAuIFNWRyBWZXJzaW9uOiA2LjAwIEJ1aWxkIDApICAtLT4NCjxzdmcgdmVyc2lvbj0iMS4xIiBpZD0iTGF5ZXJfMSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgeD0iMHB4IiB5PSIwcHgiDQoJIHdpZHRoPSI3My41cHgiIGhlaWdodD0iODcuMXB4IiB2aWV3Qm94PSIwIDAgNzMuNSA4Ny4xIiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCA3My41IDg3LjE7IiB4bWw6c3BhY2U9InByZXNlcnZlIj4NCjxzdHlsZSB0eXBlPSJ0ZXh0L2NzcyI+DQoJLnN0MHtmaWxsOiMxNTA0MDA7fQ0KCS5zdDF7ZmlsbDojRjdCRTAwO30NCjwvc3R5bGU+DQo8Zz4NCgk8cGF0aCBjbGFzcz0ic3QwIiBkPSJNMzYuNyw4Ni44Yy0zLjctMS42LTIxLTkuMy0yNi42LTEzLjljLTYuNS01LjMtMTAtMTMtMTAtMjIuMVY4LjNDMTAuNSwyLjcsMjIuNSwwLDM2LjcsMHMyNi4yLDIuNywzNi42LDguMw0KCQl2NDIuNGMwLDkuMS0zLjQsMTYuOC0xMCwyMi4xQzU3LjcsNzcuNSw0MC40LDg1LjIsMzYuNyw4Ni44eiIvPg0KCTxwYXRoIGNsYXNzPSJzdDEiIGQ9Ik02OC45LDEwQzY0LjUsOS42LDYwLDkuMyw1NS4zLDkuM0MzOC4yLDkuMywxOS44LDEzLDQuNiwyNi45djIzLjhjMCw3LjgsMi45LDE0LjIsOC4zLDE4LjcNCgkJYzQuOCwzLjksMTkuNiwxMC42LDIzLjgsMTIuNWM0LjEtMS44LDE4LjktOC40LDIzLjgtMTIuNWM1LjUtNC41LDguMy0xMC44LDguMy0xOC43VjEwIE03LjgsNDguM1YyOC45SDE0djE5LjYNCgkJYzAsMS43LDAuNCw0LjMsMy4yLDQuM2MxLjIsMCwyLjItMC4zLDIuOS0wLjhWMjguOWg2LjF2MjYuN2MtMi40LDEuNi01LjUsMi41LTkuMywyLjVDMTAuOCw1OC4xLDcuOCw1NC44LDcuOCw0OC4zIE0zNS41LDcwLjYNCgkJaC02LjJWMzAuNWMyLjItMS40LDQuOS0yLjIsOC4zLTIuMkM0NSwyOC4zLDQ5LDMzLjksNDksNDIuOGMwLDktMy45LDE1LjEtMTAuOCwxNS4xYy0xLjMsMC0yLjItMC4yLTIuOC0wLjNMMzUuNSw3MC42TDM1LjUsNzAuNnoNCgkJIE0zNS41LDUyLjZjMC40LDAuMiwxLjIsMC40LDIsMC40YzMuNSwwLDUuMi0zLjMsNS4yLTEwYzAtNi45LTEuNS05LjctNS4xLTkuN2MtMC44LDAtMS43LDAuMi0yLjIsMC40TDM1LjUsNTIuNkwzNS41LDUyLjZ6DQoJCSBNNTAuNCwzNi4zYzAtNSw0LjItNy45LDguMy04YzMuNC0wLjEsNS43LDEuMiw2LjgsMnY1LjRjLTEuMy0xLjMtMy40LTIuNS01LjUtMi41Yy0xLjcsMC0zLjYsMC44LTMuNiwyLjljLTAuMSwyLjIsMS44LDMuMSw0LDQuNQ0KCQljNS4xLDMsNi4xLDUuNyw2LDkuM2MtMC4xLDMuOS0yLjgsOC4yLTguOSw4LjJjLTIuNCwwLTQuOC0wLjgtNi44LTEuOXYtNS43YzEuNiwxLjQsMy45LDIuNSw1LjksMi41YzIuMywwLDMuNy0xLjMsMy43LTMuNA0KCQljMC0xLjktMS4xLTMtMy44LTQuNkM1MS40LDQyLDUwLjUsMzkuNiw1MC40LDM2LjMgTTEwLjksNzEuOWMtNi4yLTUuMS05LjUtMTIuNC05LjUtMjEuMlY5LjFjOS44LTUuMiwyMS43LTcuOSwzNS40LTcuOQ0KCQlzMjUuNiwyLjYsMzUuNCw3Ljl2NDEuN2MwLDguOC0zLjMsMTYuMS05LjUsMjEuMkM1Nyw3Ni43LDM4LjksODQuNiwzNi44LDg1LjVDMzQuNyw4NC41LDE2LjYsNzYuNiwxMC45LDcxLjl6IE01OS4zLDgwLjd2My42SDYwDQoJCXYtMS41aDAuMWMwLDAuMSwxLDEuNSwxLDEuNWgwLjhjMCwwLTEtMS40LTEuMS0xLjZjMC41LTAuMSwwLjgtMC41LDAuOC0xcy0wLjMtMS4xLTEuMi0xLjFMNTkuMyw4MC43TDU5LjMsODAuN3ogTTYwLjMsODEuMg0KCQljMC42LDAsMC42LDAuMywwLjYsMC41YzAsMC4zLTAuMSwwLjYtMC44LDAuNkg2MHYtMS4xSDYwLjN6IE02Mi44LDgyLjVjMCwxLjMtMS4xLDIuNC0yLjQsMi40Yy0xLjMsMC0yLjQtMS4xLTIuNC0yLjQNCgkJczEuMS0yLjUsMi40LTIuNVM2Mi44LDgxLjEsNjIuOCw4Mi41IE02MC40LDc5LjVjLTEuNiwwLTMsMS4zLTMsM2MwLDEuNiwxLjMsMywzLDNjMS42LDAsMy0xLjMsMy0zQzYzLjMsODAuOCw2Miw3OS41LDYwLjQsNzkuNSINCgkJLz4NCjwvZz4NCjwvc3ZnPg0K')

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
	SELECT u.Email, u.Name, u.UsersID FROM System.Users u WHERE u.Email = @Email AND u.Password = @Password
END;

GO

CREATE PROCEDURE Tracking.InsertPackage
@CompanyID INT = NULL,
@TrackingNumber varchar(max),
@UsersID INT = NULL,
@Name varchar(100) = NULL
AS
BEGIN
	INSERT INTO Tracking.Package (CompanyID, TrackingNumber, Name, UsersID) VALUES (@CompanyID, @TrackingNumber, @Name,  @UsersID)
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