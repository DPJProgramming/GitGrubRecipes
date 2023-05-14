
--create a couple recipes

--created by user 020f4514-2151-42ef-9c5d-efdd9cd53ae1
--INSERT INTO Recipe(Title, UserId, ImageUrl, Ingredients, Directions)
--VALUES('Muffin', '020f4514-2151-42ef-9c5d-efdd9cd53ae1', 'www.google.com', '1 cup flour, 1cup sugar', 'Mix the flour and sugar together')

--INSERT INTO Recipe(Title, UserId, ImageUrl, Ingredients, Directions)
--VALUES('Pie', '020f4514-2151-42ef-9c5d-efdd9cd53ae1', 'www.yahoo.com', '1 cup flour, 1cup sugar, 1/2 cup fruit', 'Mix the flour and sugar together. Then add the fruit in a pie dish')

--created by user '2ec5fd20-ca80-40be-8257-c53d041c4f41'
--INSERT INTO Recipe(Title, UserId, ImageUrl, Ingredients, Directions)
--VALUES('FavoriteCookie', '2ec5fd20-ca80-40be-8257-c53d041c4f41', 'www.yahoo.com', '1 cup flour, 1cup butter, 1/2 cup sugar', 'Make some cookie dough')


--a second registered user who made a recipe
--INSERT INTO Recipe(Title, AuthorId, ImageUrl, Directions)
--VALUES('cookie', '2ec5fd20-ca80-40be-8257-c53d041c4f41', 'www.cookie.com', 'Mix the flour and sugar together. Then add the fruit in a pie dish')

--add some favorite recipes
INSERT INTO FavoriteRecipes(UserId, RecipeId)
Values('2ec5fd20-ca80-40be-8257-c53d041c4f41', 1)


INSERT INTO FavoriteRecipes(UserId, RecipeId)
Values('020f4514-2151-42ef-9c5d-efdd9cd53ae1', 3)


