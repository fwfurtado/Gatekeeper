# Package Sub System

<details>
<summary>1- Cadastrar Encomenda </summary>
  
> (parecido com o que é feito com a Unit)
  
### CORE
  
  - Criar Entity Package
      - Atributos
        - Id
        - Description
        - ArrivedAt
        - DeliveredAt
        - Status 
          - Pending
          - Delivered
          - Rejected
            
    - Construtor
  
  - Criar Entity PackageStatus
      - Listar status
        
  - Criar Service Package
    - Actions
      - Register Package
      - Get Package by Id
      - List all packages
      - Delete a package
      - Update status Delivered
      - Update status Rejected
     
  - Criar Repository Package
    - Queries
      - SaveAsync => Salvar uma encomenda na tabela packages => INSERT INTO 
      - GetByIdAsync => Buscar o Id de uma encomenda na tabela packages => SELECT
      - GetAll => Buscar todas as encomendas, listando seu Id e descrição
      - DeleteByIdAsync => Deletar uma encomenda utilizando seu id
      - UpdateStatus => Atualizo a coluna status. Se for Delivered, então atualizo a data delivered_at. Caso Reject, só atualiza o status
    
  - Criar uma tabela Package
    > (add in Migrations)
    - Tabela
      - Deve conter as colunas id, description, arrived_at, delivered_at, status
     
      | id  | description | arrived_at | delivered_at | status | 
      | ------------- | ------------- | ------------- | ------------- | ------------- |
      | 1  | Pacote X  | 02/11/2023 | 03/11/2023 | Delivered |

  ### Rest 
  > (colocar exemplo de json request;response. Add método e path 'completo'- sem host)
  
  - Criar Controller Package
    - Paths
      - POST CreatPackage
      - GET ListPackages
      - GET ShowPackages
      - DELETE DeletePackages
      - PATCH UpdateDeliveredStatus
      - PATCH UpdateRejectedStatus
  - Atualizar program.cs
    - Atualizar arquivo com as relacoes de Package, Service e Command do Core
    
</details>

<details>    
<summary>2- Associar Encomenda a uma Unit</summary>
  
### CORE
  - Criar Entity Package request
  - Criar New package command
  - Varificar como fazer a chamada de PackageAddition (paralelo: RequestOccupationAsync + ApproveRequestAsync)
  - Criar Repository para esta função
  - Criar Service para esta função
  - Criar Command para esta função (?)
</details>
 <details>
<summary>Frontend</summary>
  
  - Criar Service Package
  - Criar Dtos de request e response Package
  - Criar Validation Package
  - Page Package
  - Atualizar program.cs
</details>
<details>
<summary>3- Casos de Teste </summary>
  
> (montar tabela com entrada (json request), composição da tabela, saida (json response))

| Json entrada  | Tabela atualizada | Json saída

## Test case for person creation
Considering that our database has this state
| id  | name |
| ------------- | ------------- |
| 1  | Fernando  |


When systemA  receives a new request POST /people
With request payload
{"name": "Adam"} 
Should process this request and change database to this state


| id  | name |
| ------------- | ------------- |
| 1  | Fernando  |
| 2  | Adam      |


And responds with this body
{"id": 2, "name": "Adam"}
