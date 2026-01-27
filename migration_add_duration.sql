-- Migration: Add Duration column to Speeches table
-- Run this script to add duration tracking for WPM calculation

ALTER TABLE Speeches ADD Duration INT NULL;
-- Duration stored in milliseconds
-- NULL values are allowed for existing records (they'll be excluded from WPM calculation)
