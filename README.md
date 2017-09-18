# Uxtrata

Uxtrata Web Application using EF6, MVC, and MySql as a back end. Jquery was used as a client side language. Due to time frames, there are
enhancements and features to be implemented. All this missing features will be discussed at the moment of the presentation.

Any comments or feedback will be appreciate.

Add rdlc generation using Stored procedure

CREATE DEFINER=`root`@`localhost` PROCEDURE `ReportStudentTransactions`(
   param_studentID int(11)
)
BEGIN
	SELECT t.TransactionID as 'TransactionID', bt.Name as 'TransactionType', 
        c.CourseName as 'CourseName', da.Code as 'DebitAccountCode',
		ca.Code as 'CreditAccountCode', t.Amount as 'Amount' 
	FROM students s
    INNER JOIN courseselections cs ON s.StudentID = cs.StudentID
    INNER JOIN courses c ON cs.CourseID = c.CourseID
    INNER JOIN transactions t on cs.ID = t.BusinessID
    INNER JOIN businesstypes bt on t.businesstypeID = bt.businesstypeID
    INNER JOIN accounts da on t.debitAccountID = da.accountID
    INNER JOIN accounts ca on t.creditAccountID = ca.accountID
    WHERE s.StudentID = param_studentID and bt.name = 'Enroll_Student';
END

Thank you

Cristian

