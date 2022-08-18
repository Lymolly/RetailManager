CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
begin
set nocount on
	select [Id], [ProductId], [Quntity], [PurchasePrice], [PurchaseDate]
	from dbo.Inventory
end
