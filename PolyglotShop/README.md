# PolyglotShop – Persistência Poliglota (EasyShop)

Protótipo de mini e-commerce que usa **dois bancos locais ao mesmo tempo** no mesmo app (.NET MAUI / Android):

| Domínio  | Paradigma            | Tecnologia            | Arquivo local     |
|----------|----------------------|-----------------------|-------------------|
| Produtos | NoSQL (documentos)   | LiteDB                | `products.db`     |
| Clientes | SQL (relacional)     | SQLite (sqlite-net)   | `customers.db3`   |

## Integrantes
- Luis Felipe
- Thainá Mariano
- Paulo Henrique

## Por que poliglota?
- **Clientes** exigem consistência: tabela com PK autoincrement, `NOT NULL` e e-mail `UNIQUE`.
- **Produtos** têm atributos variáveis (camisa: tamanho/cor; notebook: CPU/RAM). Em SQL isso viraria colunas nulas ou tabela EAV; no documento BSON basta um `Dictionary<string,string> Attributes`, sem alterar schema.

## Arquitetura em camadas
```
Views/ + ViewModels/   → Apresentação (UI, sem acesso a banco)
Services/              → Negócio (validações, parse de atributos, regras)
Data/                  → Acesso a dados (CustomerRepository = SQLite | ProductRepository = LiteDB)
Models/                → Customer (tabela) e Product (documento)
```
Fluxo: `View → ViewModel → Service → Repository → Banco`. Dependências injetadas em `MauiProgram.cs` via interfaces.

## Funcionalidades
- **Aba Produtos (NoSQL):** nome, preço, categoria e atributos opcionais no formato `Cor=Azul; Tamanho=M`; listagem vinda do LiteDB.
- **Aba Clientes (SQL):** nome completo e e-mail (validação de formato e duplicidade); listagem vinda do SQLite.

## Como executar
Pré-requisitos: .NET 8 SDK, workload MAUI (`dotnet workload install maui-android`), Android SDK e um emulador Android.

```bash
git clone <URL-DO-REPOSITORIO>
cd PolyglotShop
dotnet build -t:Run -f net8.0-android
```
Ou abra o `.csproj` no Visual Studio 2022 / VS Code, escolha o emulador Android e execute.

## Exemplo de teste
- Produto: `Notebook X` | `4599,90` | `Informática` | `Processador=i7; RAM=16GB; Garantia=12 meses`
- Produto: `Camisa Polo` | `89,90` | `Vestuário` | `Tamanho=M; Cor=Azul` (atributos diferentes, mesma coleção)
- Cliente: `Maria da Silva` | `maria@email.com`
