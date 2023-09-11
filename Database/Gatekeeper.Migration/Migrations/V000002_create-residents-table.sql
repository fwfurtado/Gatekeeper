CREATE TABLE residents
(
    id         BIGSERIAL    NOT NULL,
    document VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,

    CONSTRAINT residents_pkey PRIMARY KEY (id),
    UNIQUE(document)

);