# 💰 Wallet Transactions App

A full-stack wallet application that allows users to create and view financial transactions (deposits and withdrawals) with real-time updates.

---

## 🚀 Tech Stack

### Frontend

* Vue (Vite)
* TypeScript
* Fetch API

### Backend

* .NET (C#)
* Entity Framework Core
* SQLite (or your DB)

---

## 📁 Project Structure

```
root/
  api/        # .NET backend
  frontend/   # Vue frontend
```

---

## ⚙️ Features

* Create transactions (deposit / withdraw)
* View transaction history
* Real-time UI updates after transactions
* Clean separation between frontend and backend

---

## 🛠️ Setup Instructions

### 1. Clone the repository

```
git clone <your-repo-url>
cd <repo-name>
```

---

### 2. Run Backend (API)

```
cd api
dotnet restore
dotnet run
```

API will run on:

```
http://localhost:5000
```

---

### 3. Run Frontend

Open a new terminal:

```
cd frontend
npm install
npm run dev
```

Frontend will run on:

```
http://localhost:5173
```

---

## 🔌 API Endpoints

### Get Transactions

```
GET /api/transactions
```

### Create Transaction

```
POST /api/transactions
```

Example body:

```
{
  "amount": 100,
  "type": "deposit"
}
```

---

## 🧠 How It Works

1. User submits a transaction from the frontend
2. Frontend sends a POST request to the API
3. API saves the transaction to the database
4. Frontend refetches transactions
5. UI updates with latest data

---

## ✅ Status

✔ Core functionality complete
✔ Fullstack integration working
✔ Ready for further enhancements or deployment

---

## 👨‍💻 Author

Built as a full-stack learning project.
