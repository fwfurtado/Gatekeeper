ALTER TABLE units
    ADD COLUMN occupation_id BIGINT NULL, 
    ADD CONSTRAINT fk_units_occupation_id_occupations FOREIGN KEY (occupation_id) REFERENCES occupations(id);