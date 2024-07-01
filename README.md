# Financial Tracker

This is a back-end for a Financial-tracker, an app I made to learn React and to learn the basics of CRUD RESTful API in C#.

## Technologies

1. ASP.NET 8
2. SQLite
3. dotnet CLI

## Usage

Simply clone the repository and run the command:

```bash
dotnet run --project Finance
```

## API Definition

### Create Expense

#### Request

```
POST /expenses
```

```json
{
  "title": "Groceries",
  "amount": 279.7,
  "date": "2024-06-08T08:00:00"
}
```

#### Response

```
201 Created
```

```
Location: {{host}}/expenses/{{id}}
```

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Groceries",
  "amount": 279.7,
  "date": "2024-06-08T08:00:00"
}
```

### Get Expenses

#### Get All Request

```
GET /expenses
```

#### Response

```
200 OK
```

```json
{
  "expenses": [
    {
      "id": "00000000-0000-0000-0000-000000000000",
      "title": "Groceries",
      "amount": 279.7,
      "date": "2024-06-08T08:00:00"
    },
    {
      "id": "00000000-0000-0000-0000-000000000001",
      "title": "Tea",
      "amount": 29,
      "date": "2024-06-08T08:00:00"
    },
    {
      "id": "00000000-0000-0000-0000-000000000002",
      "title": "Lollipop",
      "amount": 7,
      "date": "2024-06-08T08:00:00"
    }
  ]
}
```

#### Get One Request

```
GET /expenses/{{id}}
```

#### Response

```
200 OK
```

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Groceries",
  "amount": 279.7,
  "date": "2024-06-08T08:00:00"
}
```

### Update Expense

#### Request

```
PUT /expenses/{{id}}
```

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "title": "Groceries",
  "amount": 279.7,
  "date": "2024-06-08T08:00:00"
}
```

#### Response

```
204 No Content
```

or

```
201 Created
```

```
Location: {{host}}/expenses/{{id}}
```

### Delete Expense

#### Request

```
DELETE /expenses/{{id}}
```

#### Response

```
204 No Content
```
