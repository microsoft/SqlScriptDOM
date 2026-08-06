-- Regression tests for GitHub issue #216: SPARSE combined with MASKED WITH must parse in
-- either clause order. The order documented by the CREATE TABLE reference is SPARSE first,
-- then MASKED WITH.

-- Documented order: SPARSE before MASKED WITH (this is the case that used to fail).
CREATE TABLE t (c VARCHAR(100) SPARSE MASKED WITH (FUNCTION = 'default()') NULL);

-- Reversed order: MASKED WITH before SPARSE (this already worked).
CREATE TABLE t (c VARCHAR(100) MASKED WITH (FUNCTION = 'default()') SPARSE NULL);

-- SPARSE FILESTREAM storage followed by MASKED WITH.
CREATE TABLE t (c VARBINARY(MAX) FILESTREAM SPARSE MASKED WITH (FUNCTION = 'default()') NULL);
