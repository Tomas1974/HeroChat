


DROP TABLE IF EXISTS messages;


create table messages
(
    id      SERIAL PRIMARY KEY,
    ChatFrom    text  NOT NULL,
    RoomId     integer   not NULL,
    ChatMessage        text   not NULL
    
);

INSERT INTO messages (ChatFrom,RoomId,ChatMessage)
VALUES
('Superman', 1, 'Hej med dig flæskesteg'),
('Spiderman', 1, 'Flæskesteg ikke til mig'),
('Superman', 1, 'Du kan ikke sige nej'),
('Spiderman', 1, 'Så går jeg min vej');