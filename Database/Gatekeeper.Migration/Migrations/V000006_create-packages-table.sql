CREATE TABLE packages
(
    id         BIGSERIAL    NOT NULL,
    description VARCHAR(255) NOT NULL,
    arrived_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    delivered_at TIMESTAMP WITHOUT TIME ZONE,
    status VARCHAR(255) NOT NULL,

    CONSTRAINT packages_pkey PRIMARY KEY (id)

);