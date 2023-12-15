CREATE TABLE package_events_history
(
    id         BIGSERIAL NOT NULL,
    package_id BIGINT    NOT NULL,
    occured_at TIMESTAMP NOT NULL,
    event_type VARCHAR   NOT NULL,
    metadata   JSONB     NOT NULL,

    CONSTRAINT package_events_history_pkey PRIMARY KEY (id),
    CONSTRAINT uq_package_events_history UNIQUE (package_id, occured_at),
    CONSTRAINT fk_package FOREIGN KEY (package_id) REFERENCES packages (id)
);