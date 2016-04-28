CREATE TABLE [dbo].[TaskProgress]
(
	[Id] INT NOT NULL  IDENTITY(1,1) PRIMARY KEY, 
    [OrderId] INT NOT NULL,
	[State] INT NOT NULL DEFAULT 0, 
    [Description] NVARCHAR(MAX) NULL, 
    [DateFrom] DATETIME NOT NULL, 
    [DateTo] DATETIME NULL, 
    [Price] FLOAT NULL, 
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
)
