﻿CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
Begin
SET NOCOUNT ON;

	SELECT p.Id,p.ProductName,p.RetailPrice,p.Description,p.QuantityInStock 
	FROM dbo.Product p
	ORDER BY ProductName;
END
