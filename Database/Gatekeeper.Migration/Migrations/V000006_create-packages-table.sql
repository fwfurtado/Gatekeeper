CREATE TABLE packages
(
    id         BIGSERIAL    NOT NULL,
    description VARCHAR(255) NOT NULL,
    arrived_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    delivered_at TIMESTAMP WITHOUT TIME ZONE,
    status VARCHAR(255) NOT NULL,
    target_unit_id BIGINT NOT NULL,

    CONSTRAINT packages_pkey PRIMARY KEY (id),
    CONSTRAINT fk_occupations_unit FOREIGN KEY (target_unit_id) REFERENCES units(id)

);