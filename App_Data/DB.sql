CREATE TABLE [dbo].[Order]
(
	[OrderID] INT NOT NULL PRIMARY KEY Identity(1,1),
    [OrderNo] varchar(20) not null,
    [OrderDate] datetime not null,
    [Description] varchar(300) null
)


CREATE TABLE [dbo].[OrderDetails]
(
	[OrderItemsID] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [OrderID] INT not null, 
    [ItemName] VARCHAR(50) NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Rate] NUMERIC(12, 2) NOT NULL, 
    [TotalAmount] NUMERIC(12, 2) NOT NULL, 
    CONSTRAINT [FK_OrderDetails_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order]([OrderId]),

)


