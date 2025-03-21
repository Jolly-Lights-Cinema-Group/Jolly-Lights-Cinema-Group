PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Employee (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    DateOfBirth TEXT NOT NULL,
    Address TEXT NOT NULL,
    Email TEXT NOT NULL,
    UserName TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL
);
