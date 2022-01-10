CREATE TABLE [dbo].[SaleDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SaleId] INT NOT NULL,
	[ProductId] INT NOT NULL,
	[PurchasePrice] MONEY NOT NULL,
	[Tax] MONEY NOT NULL DEFAULT 0,
	[Quantity] INT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_SaleDetails_ToSale] FOREIGN KEY (SaleId) REFERENCES Sale(Id), 
    CONSTRAINT [FK_SaleDetails_ToProduct] FOREIGN KEY (ProductId) REFERENCES Product(Id)
)
