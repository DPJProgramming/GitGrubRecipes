--INSERT INTO Recipe(Title, UserId, ImageUrl, Ingredients, Directions)
--VALUES('Muffin', 1, 'www.google.com', '1 cup flour, 1cup sugar', 'Mix the flour and sugar together')

--INSERT INTO Recipe(Title, UserId, ImageUrl, Ingredients, Directions)
--VALUES('Pie', 2, 'www.yahoo.com', '1 cup flour, 1cup sugar, 1/2 cup fruit', 'Mix the flour and sugar together. Then add the fruit in a pie dish')

--a second registered user who made a recipe
INSERT INTO Recipe(Title, AuthorId, ImageUrl, Directions)
VALUES('cookie', '2ec5fd20-ca80-40be-8257-c53d041c4f41', 'www.cookie.com', 'Mix the flour and sugar together. Then add the fruit in a pie dish')
