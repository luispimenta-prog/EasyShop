# PolyglotShop – Persistência Poliglota (EasyShop)
A proposta é um mini e-commerce em **.NET MAUI (Android)** que usa **dois bancos de dados locais ao mesmo tempo**, cada um no tipo de dado em que faz mais sentido. Isso é o que chamamos de *persistência poliglota*.

## Integrantes
- Luis Felipe Venâncio Pimenta
- Thainá Tavares Mariano
- Paulo Henrique da Silva Grilo

## Vídeo de demonstração
(https://drive.google.com/file/d/1shvY4Iq4PZd5rSQr8C0T8BI08dvrp5VQ/view?usp=sharing)

## Sobre o projeto
O app tem duas telas (abas), e cada uma usa um banco diferente:

| Domínio  | Paradigma            | Tecnologia            | Arquivo local     |
|----------|----------------------|-----------------------|-------------------|
| Produtos | NoSQL (documentos)   | LiteDB                | `products.db`     |
| Clientes | SQL (relacional)     | SQLite (sqlite-net)   | `customers.db3`   |

### Por que usamos dois bancos?
- **Clientes:** os dados são bem estruturados e precisam de consistência. Por isso usamos uma tabela com chave primária autoincrement, campos `NOT NULL` e e-mail `UNIQUE`, o que o SQL resolve muito bem.
- **Produtos:** cada tipo de produto tem atributos diferentes (uma camisa tem tamanho e cor, um notebook tem processador e RAM). Em SQL isso geraria várias colunas vazias ou uma tabela EAV, que é mais complicada. No LiteDB (documento BSON) basta um `Dictionary<string,string> Attributes`, e não precisamos mudar o schema para cadastrar um produto novo.

## Arquitetura
Organizamos o código em camadas para separar as responsabilidades:

```
Views/ + ViewModels/   → Apresentação (telas, sem acesso direto ao banco)
Services/              → Regras de negócio (validações, leitura dos atributos)
Data/                  → Acesso a dados (CustomerRepository = SQLite | ProductRepository = LiteDB)
Models/                → Customer (tabela) e Product (documento)
```

Fluxo de uma operação: `View → ViewModel → Service → Repository → Banco`.
As dependências são injetadas no `MauiProgram.cs` através de interfaces.

## Funcionalidades
- **Aba Produtos (NoSQL):** cadastro com nome, preço, categoria e atributos opcionais no formato `Cor=Azul; Tamanho=M`. A listagem vem do LiteDB.
- **Aba Clientes (SQL):** cadastro com nome completo e e-mail, com validação de formato e de e-mail duplicado. A listagem vem do SQLite.

## Como executar

### Pré-requisitos
- .NET 8 SDK
- Workload do MAUI para Android: `dotnet workload install maui-android`
- Android SDK
- Um emulador Android criado (pelo Android Studio ou pelo Visual Studio)

### Passo a passo
1. Clonar o repositório e entrar na pasta do projeto:
```bash
git clone https://github.com/luispimenta-prog/EasyShop.git
cd EasyShop/PolyglotShop
```
2. Deixar o emulador Android aberto e rodando.
3. Compilar e executar:
```bash
dotnet build -t:Run -f net8.0-android
```

**Alternativa pela IDE:** abrir o arquivo `.csproj` no Visual Studio 2022 ou no VS Code, escolher o emulador Android como destino e clicar em executar.

## Exemplos para testar
- **Produto:** `Notebook X` | `4599,90` | `Informática` | `Processador=i7; RAM=16GB; Garantia=12 meses`
- **Produto:** `Camisa Polo` | `89,90` | `Vestuário` | `Tamanho=M; Cor=Azul`
  (os atributos são diferentes dos do notebook, mas os dois ficam na mesma coleção)
- **Cliente:** `Maria da Silva` | `maria@email.com`
  (se tentar cadastrar o mesmo e-mail de novo, o app barra a duplicidade)

## Conclusão
Com esse trabalho deu para ver na prática que não existe um banco "melhor" para tudo: o SQL foi mais adequado para os clientes, por causa das regras e da consistência, e o NoSQL foi mais adequado para os produtos, por causa da flexibilidade dos atributos. Usar os dois no mesmo app deu mais trabalho na configuração, mas deixou o modelo de dados mais natural para cada caso.
