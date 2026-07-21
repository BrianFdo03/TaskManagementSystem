# TaskManagementSystem

Step 6 — Add Setup Instructions for README

This is IMPORTANT for your submission.

Add this section:

Database Setup
### 1. Create Database

Run in SQL Server:

CREATE DATABASE TaskManagementDB;


### 2. Run Table Scripts

Execute:

database/Scripts/02_create_tables.sql


### 3. Run Seed Data

Execute:

database/Scripts/01_seed_data.sql


### 4. Verify Data
SELECT * FROM Users;
SELECT * FROM Tasks;