INSERT INTO dbo.Users(UserID, FirstName, LastName, isSubbed)
Values (1, 'Patrick', 'Graska', 1),
(2, 'Jarrid', 'Fisher', 1),
(3, 'Jacob', 'Mollman', 1),
(4, 'Hunter', 'Gulley', 1),
(5, 'JC', 'Pyron', 1),
(6, 'Austin', 'Reifsteck', 0);

INSERT INTO dbo.addresses(User_UserID, StreetAddress, city, state, zip)
VALUES 
(1, '5793 Rutledge Trail', 'Liberty Township', 'OH', '45011'),
(2, '6509 Frontier Ct.', 'Liberty Township', 'OH', '45044'),
(3, '6875 Hidden Ridge Dr', 'West Chester', 'OH', '45069'),
(4, '8291 Twin Cove Ct.', 'West Chester', 'OH', '45069'),
(5, '6282 Foxwood Ct.', 'Liberty Township', 'OH', '45044'),
(6, '6606 Fountains Blvd.', 'West Chester', 'OH', '45069');

ALTER TABLE dbo.SMtype
ALTER COLUMN Type nvarchar(50);

INSERT INTO dbo.SMtype(SocialMedia_SMtyKey, Type)
VALUES 
(1, 'Twitter'),
(2, 'Facebook'),
(3, 'Email');


INSERT INTO dbo.SocialMedia( User_UserID, SMHandle, SMtyKey)
VALUES
(1, 'patrickgraska@gmail.com', 2),
(2, 'Jrock5499', 1),
(2, 'Jrock5499@gmx.com', 2),
(3, 'Jacob Mollman', 1),
(3, 'jbmollman@gmail.com', 2),
(4, 'Krypton_Gaming', 1),
(4, 'hgulley1234@gmail.com', 2),
(5, 'jcpyron', 1),
(5, 'jcpyron@fuse.net', 2),
(6, 'apreifsteck@yahoo.com' ,2);


INSERT INTO dbo.Email
VALUES 
(1, 'patrickgraska@gmail.com'),
(2, 'jarrid.fisher@gmail.com'),
(3, 'jbmollman@gmail.com'),
(4, 'kryptongaming@gmail.com'),
(5, 'jcpyron@gmail.com'),
(6, 'apreifsteck@gmail.com');


INSERT INTO dbo.Twitter
VALUES
(1,'4889317679-n1uonOWCke2afCzKzD4zRBd6xpDzqihe6vJUPD4','5CSuXHhMIsKcxnVJbFlyOb404xuRUWrOtFfuO6hOO8iAM',4889317679,'semblaster');

INSERT INTO dbo.Face
VALUES
(1,'CAAYXa92BOoEBAB18pCP0RMQHKPj5XKmisAWHptZC4qeKjiNXfDXaIjp5y8WSInEMu4WBB65x0L7fZCQe0iLk8JIOngUSxHlFwtBgXoILCfnYf7qAy829ytcyxoCdoe8J5uuuLfSsb7VCuNFqnJh2UACJdqhjHclpU6h2Elf7Y862kZA9gDTcVrQB9GsOl8ZD');

INSERT INTO dbo.UMail
VALUES
(1,'socialmediablaster@gmail.com', 'theenthusiasmofjc');
