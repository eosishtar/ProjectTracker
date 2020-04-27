/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


-- Effort
MERGE INTO ptm.Effort AS T 
USING (VALUES (1, '1 Day', 1),
			  (2, '2 Days', 2),
			  (3, '3 Days', 3),
			  (4, '4 Days', 4),
			  (5, '5 Days', 5),
			  (6, '6 Days', 6),
			  (7, '1 Week', 7),
			  (8, '2 Weeks', 14),
			  (9, '3 Weeks', 21),
			  (10, '1 Month', 30),
			  (11, '2 Month', 60),
			  (12, '3 Month', 90),
			  (13, '4 Month', 120))

 AS S(Id, Effort, ValueInMinutes)
ON T.Id = S.Id AND T.[Description] = S.Effort 
WHEN MATCHED AND EXISTS (SELECT T.Description EXCEPT SELECT S.Effort)
THEN 
UPDATE SET 
	Description = S.Effort,
	ValueInMinutes = S.ValueInMinutes
WHEN NOT MATCHED BY TARGET THEN
	INSERT VALUES(Id, Effort, ValueInMinutes)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
GO