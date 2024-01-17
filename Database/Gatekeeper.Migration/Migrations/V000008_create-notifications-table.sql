CREATE TABLE notifications
(
    id         BIGSERIAL NOT NULL,
    type       VARCHAR   NOT NULL,
    payload    JSONB     NOT NULL,
    created_at TIMESTAMP NOT NULL,

    CONSTRAINT notifications_pkey PRIMARY KEY (id)
);
