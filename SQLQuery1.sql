CREATE TABLE users
(
	id INT PRIMARY KEY IDENTITY(1,1),
	email VARCHAR(MAX) NULL,
	username VARCHAR(MAX) NULL,
	password VARCHAR(MAX) NULL,
	date_regiser DATE NULL
)

SELECT * FROM users

CREATE TABLE books 
(
	id INT PRIMARY KEY IDENTITY(1,1),
	book_title VARCHAR(MAX) NULL,
	author VARCHAR(MAX) NULL,
	published_date DATE NULL,
	status VARCHAR(MAX) NULL,
	date_insert DATE NULL,
	date_update DATE NULL, 
	date_delete DATE NULL
)

SELECT * FROM books WHERE date_delete IS NULL

ALTER TABLE books
ADD image VARCHAR(MAX) NULL

SELECT * FROM books