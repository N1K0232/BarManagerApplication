﻿
CREATE VIEW ViewMenu
AS
SELECT p.Name,p.Price,p.Quantity,c.Name as Category
FROM Products p LEFT JOIN Categories c
ON p.IdCategory=c.Id
WHERE p.Quantity > 0