"# ListaTarefasAPI" 

Descrição do Projeto

O projeto ListaTarefasAPI é uma API RESTful desenvolvida em C# utilizando o framework ASP.NET Core. Essa API oferece funcionalidades para gerenciar listas de tarefas e suas respectivas tarefas. O sistema permite a criação, leitura, atualização e exclusão de listas de tarefas, bem como a adição, consulta e atualização de tarefas associadas a essas listas.

Recursos Principais:

Listas de Tarefas:
-Criar uma nova lista de tarefas.
-Visualizar detalhes de uma lista de tarefas específica.
-Listar todas as listas de tarefas disponíveis.
-Atualizar informações de uma lista de tarefas existente.
-Excluir uma lista de tarefas.

Tarefas:
-Adicionar uma nova tarefa a uma lista específica.
-Visualizar detalhes de uma tarefa específica.
-Listar todas as tarefas associadas a uma lista de tarefas.
-Atualizar informações de uma tarefa existente.
-Excluir uma tarefa.

Tecnologias Utilizadas:
-ASP.NET Core
-Entity Framework Core (EF Core)
-C# 9.0
-SQL Server (ou outro SGBD compatível)

Instruções de Uso:
-Clone o repositório.
-Configure a string de conexão com o banco de dados no arquivo de configuração.
-Execute as migrações do EF Core para criar o banco de dados.
-Execute o projeto e acesse a API por meio dos endpoints especificados na documentação.

Documentação Adicional:

-Comandos SQL necessários para o funcionamento correto do projeto:
CREATE TABLE ListasDeTarefas (
    ListaDeTarefasID INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    DataCriacao DATETIME NOT NULL,
    DataModificacao DATETIME
);

CREATE TABLE Tarefas (
    TarefaID INT IDENTITY(1,1) PRIMARY KEY,
    ListaDeTarefasID INT NOT NULL,
    Descricao VARCHAR(255) NOT NULL,
    DataCriacao DATETIME NOT NULL,
    Concluida BIT,
    FOREIGN KEY (ListaDeTarefasID) REFERENCES [dbo].[ListasDeTarefas]([ListaDeTarefasID])
);

CREATE TRIGGER AtualizarDataModificacao
ON Tarefas
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Declare uma variável para armazenar o ListaDeTarefasID da tarefa afetada
    DECLARE @ListaDeTarefasID INT

    -- Verifique se a operação é uma inserção (INSERT)
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Obtenha o ListaDeTarefasID da tarefa inserida
        SELECT @ListaDeTarefasID = ListaDeTarefasID
        FROM inserted
    END
    ELSE -- Se não é uma inserção, é uma atualização ou exclusão
    BEGIN
        -- Obtenha o ListaDeTarefasID da tarefa afetada
        SELECT @ListaDeTarefasID = ListaDeTarefasID
        FROM deleted
    END

    -- Atualize a tabela [ListasDeTarefas] correspondente com a nova data de modificação
    UPDATE ListasDeTarefas
    SET DataModificacao = GETDATE() -- Use GETDATE() para obter a data e hora atuais
    WHERE ListaDeTarefasID = @ListaDeTarefasID
END

-Exemplos de buscas para realizar testes no banco de dados:
Select * from [dbo].[ListasDeTarefas];
Select * from [dbo].[Tarefas];

SELECT
    L.ListaDeTarefasID,
    L.Nome AS NomeDaLista,
    T.TarefaID,
    T.Descricao AS DescricaoDaTarefa,
    T.Concluida
FROM
    [dbo].[ListasDeTarefas] AS L
JOIN
    [dbo].[Tarefas] AS T ON L.ListaDeTarefasID = T.ListaDeTarefasID;
