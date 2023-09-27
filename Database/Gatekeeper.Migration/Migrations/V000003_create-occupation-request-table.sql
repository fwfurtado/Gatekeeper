CREATE TABLE occupation_requests (
    
    id     BIGSERIAL    NOT NULL PRIMARY KEY,
    target_unit_id BIGINT NOT NULL,
    status VARCHAR(255) NOT NULL,
    requested_at TIMESTAMP WITHOUT TIME ZONE,
    
    CONSTRAINT fk_occupation_requests_unit FOREIGN KEY (target_unit_id) REFERENCES units(id)

);

CREATE TABLE occupation_request_people (
    id     BIGSERIAL    NOT NULL PRIMARY KEY,
    occupation_id   BIGINT NOT NULL,
    name VARCHAR(255) NOT NULL,
    document VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    phone VARCHAR(255) NOT NULL,
    
    CONSTRAINT  fk_occupaton_requests_request_people FOREIGN KEY (occupation_id) REFERENCES occupation_requests(id)
);