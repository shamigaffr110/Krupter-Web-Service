
-- Create DB and table for EBillingSolution
CREATE DATABASE ElectricityBillDB;
GO
USE ElectricityBillDB;
GO

CREATE TABLE ElectricityBill (
    consumer_number VARCHAR(20) NOT NULL,
    consumer_name   VARCHAR(50) NOT NULL,
    units_consumed  INT NOT NULL,
    bill_amount     FLOAT NOT NULL
);
GO
