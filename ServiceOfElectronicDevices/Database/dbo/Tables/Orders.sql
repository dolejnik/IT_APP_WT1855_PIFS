CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [DeviceId] INT NOT NULL, 
    [Price] FLOAT NULL,
	FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY (DeviceId) REFERENCES Devices(Id)
)
