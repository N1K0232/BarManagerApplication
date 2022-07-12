CREATE VIEW Menu
AS
SELECT p.Name as Product,p.Price,p.Quantity,c.Name as Category
FROM Products p LEFT JOIN Categories c
ON p.CategoryId=c.Id
WHERE p.Quantity > 0