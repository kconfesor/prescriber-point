CREATE TABLE IF NOT EXISTS users (
                                     id SERIAL PRIMARY KEY,
                                     name VARCHAR(255),
                                     username VARCHAR(255) UNIQUE,
                                     salt BYTEA,
                                     password_hash VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS journal (
                                       id SERIAL PRIMARY KEY,
                                       user_id INT NOT NULL,
                                       patient VARCHAR(255),
                                       note TEXT,
                                       created_at TIMESTAMPTZ NOT NULL,
                                       modified_at TIMESTAMPTZ,
                                       FOREIGN KEY (user_id) REFERENCES users (id)
);