SELECT REGEXP_COUNT('hello', 'he(l+)o');
SELECT REGEXP_COUNT('hello', 'he(l+)o', 1);
SELECT REGEXP_COUNT('hello', 'he(l+)o', 1, 'i');

SELECT REGEXP_INSTR('hello', 'he(l+)o');
SELECT REGEXP_INSTR('hello', 'he(l+)o', 1);
SELECT REGEXP_INSTR('hello', 'he(l+)o', 1, 1);
SELECT REGEXP_INSTR('hello', 'he(l+)o', 1, 1, 0);
SELECT REGEXP_INSTR('hello', 'he(l+)o', 1, 1, 0, 'i');
SELECT REGEXP_INSTR('hello', 'he(l+)o', 1, 1, 0, 'c', 1);

SELECT REGEXP_REPLACE('hello', 'he(l+)o');
SELECT REGEXP_REPLACE('hello', 'he(l+)o', 'hi\1o');
SELECT REGEXP_REPLACE('hello', 'he(l+)o', 'hi\1o', 1);
SELECT REGEXP_REPLACE('hello', 'he(l+)o', 'hi\1o', 1, 1);
SELECT REGEXP_REPLACE('hello', 'he(l+)o', 'hi\1o', 1, 1, 'm');

SELECT REGEXP_SUBSTR('hello', 'he(l+)o');
SELECT REGEXP_SUBSTR('hello', 'he(l+)o', 1);
SELECT REGEXP_SUBSTR('hello', 'he(l+)o', 1, 1);
SELECT REGEXP_SUBSTR('hello', 'he(l+)o', 1, 1, 's');
SELECT REGEXP_SUBSTR('hello', 'he(l+)o', 1, 1, 'c', 1);

SELECT *
FROM EMPLOYEES
WHERE REGEXP_LIKE(FIRST_NAME, '^A.*Y$');

SELECT *
FROM ORDERS
WHERE REGEXP_LIKE(ORDER_DATE, '2020-02-\\d\\d');

SELECT *
FROM PRODUCTS
WHERE REGEXP_LIKE(PRODUCT_NAME, '[AEIOU]{3,}');

SELECT REGEXP_LIKE('hello', 'he(l+)o');

SELECT REGEXP_LIKE('hello', 'he(l+)o', 'i');