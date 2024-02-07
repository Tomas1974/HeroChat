


DROP TABLE IF EXISTS messages;




create table messages
(
    id      SERIAL PRIMARY KEY,
    ChatFrom    VARCHAR(50)  NOT NULL,
    Room     integer   not NULL,
    ChatMessage        VARCHAR(50)   not NULL
    
);

INSERT INTO messages (ChatFrom,Room,ChatMessage)
VALUES ('Superman', 1, 'Hej med dig flæskesteg');
VALUES ('Spiderman', 1, 'Flæskesteg ikke til mig');
VALUES ('Superman', 1, 'Du kan ikke sige nej');
VALUES ('Spiderman', 1, 'Så går jeg min vej');