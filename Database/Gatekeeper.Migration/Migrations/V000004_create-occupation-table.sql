CREATE TABLE occupations (
    
    id     BIGSERIAL    NOT NULL PRIMARY KEY,
    target_unit_id BIGINT NOT NULL,
    startAt DATE,
    endAt DATE,
    
    CONSTRAINT fk_occupations_unit FOREIGN KEY (target_unit_id) REFERENCES units(id)

);

CREATE TABLE occupation_members (
    id     BIGSERIAL    NOT NULL PRIMARY KEY,
    occupation_id   BIGINT NOT NULL,
    resident_id   BIGINT NOT NULL,
    
    CONSTRAINT  fk_occupaton_member_occupations FOREIGN KEY (occupation_id) REFERENCES occupations(id),
    CONSTRAINT  fk_occupaton_member_residents FOREIGN KEY (resident_id) REFERENCES residents(id)
);