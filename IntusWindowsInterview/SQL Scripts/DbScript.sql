IF OBJECT_ID('IntusWindowsREST_GetOrder', 'P') IS NOT NULL
    DROP PROCEDURE IntusWindowsREST_GetOrder
GO
CREATE PROCEDURE [dbo].[IntusWindowsREST_GetOrder]
(
	@Id INT = null
)
AS
BEGIN
    select O_D.ID AS OrderId
          , O_D.Name as OrderName
          , O_D.State as State
          , W_D.ID AS WindowId
          , W_D.Name as WindowName
          , W_D.QuantityOfWindows AS QuantityOfWindows
          , W_D.TotalSubElements AS TotalSubElements
          , S_E.ID AS SubElementId 
          , S_E.Element AS Element
          , S_E.Type AS Type
          , S_E.Width AS Width
          , S_E.Height AS Height
    from Orders O_D
    inner join Windows W_D
    on O_D.Id = W_D.OrderId
    inner join SubElements S_E
    on W_D.Id = S_E.WindowId
    where O_D.Id = @Id or @Id is null
END
GO