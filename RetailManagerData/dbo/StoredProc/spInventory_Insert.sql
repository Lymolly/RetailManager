CREATE PROCEDURE [dbo].[spInventory_Insert]
	--[ProductId], [Quntity], [PurchasePrice], [PurchaseDate]
	@ProductId int,
	@Quntity int,
	@PurchasePrice money,
	@PurchaseDate datetime2
AS
begin 
	set nocount on
	insert into dbo.Inventory(ProductId,Quntity,PurchasePrice,PurchaseDate)
	values(@ProductId,@Quntity,@PurchasePrice,@PurchaseDate)
end
