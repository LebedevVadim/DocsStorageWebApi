# DocsStorageWebApi
Указать папку хранения документов и строку подключения в appsettings.json

Создать две хранимые процедуры:

1. 
create procedure GetDocuments
@offset int,
@count int
as 
select 
*
from Documents 
order by 
Documents.LoadDate desc
offset @offset rows
fetch next @count rows only

2. 
create procedure GetDocument
@id int
as 
select
*
from Documents where @id = Documents.DocumentID

# DocsStorage
Непонятный баг с тем, что если отправлять файл, у которого есть в названии русские буквы - тело документа не записывается. Не разобрался в чем дело
