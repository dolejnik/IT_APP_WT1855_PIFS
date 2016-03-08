CREATE TABLE [dbo].[TaskProgress]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [OrderId] INT NOT NULL,
	[State] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [DateFrom] DATETIME NOT NULL, 
    [DateTo] DATETIME NULL, 
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
)
