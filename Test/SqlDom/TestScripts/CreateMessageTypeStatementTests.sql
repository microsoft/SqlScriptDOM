CREATE MESSAGE TYPE [//Adventure-Works.com/Expenses/SubmitExpense] VALIDATION = WELL_FORMED_XML
CREATE MESSAGE TYPE m1 authorization zzz VALIDATION = none
CREATE MESSAGE TYPE m1 VALIDATION = empty
CREATE MESSAGE TYPE m1  VALIDATION = VALID_XML WITH SCHEMA COLLECTION sch1