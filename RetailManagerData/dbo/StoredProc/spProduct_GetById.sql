
CREATE PROCEDURE [dbo].[spProduct_GetById]
@Id int
AS
BEGIN
SET NOCOUNT ON;
	SELECT p.Id,p.ProductName,p.RetailPrice,p.Description,p.QuantityInStock,p.IsTaxable
	FROM dbo.Product p WHERE p.Id = @Id
END