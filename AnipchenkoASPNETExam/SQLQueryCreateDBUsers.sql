﻿--CREATE TABLE [users](id INT IDENTITY PRIMARY KEY, [login] VARCHAR(50) NOT NULL, [pass] VARCHAR(50) NOT NULL, [name] VARCHAR(50) NOT NULL, [surname] VARCHAR(50) NOT NULL, [imageurl] VARCHAR(150))

--CREATE TABLE [admins](id INT IDENTITY PRIMARY KEY, [userID] INT FOREIGN KEY REFERENCES [users](id))

--DROP TABLE [admins]

--DROP TABLE [users]