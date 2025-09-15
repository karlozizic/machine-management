--
-- PostgreSQL database dump
--

\restrict x7HeKaWcRKQRy8xv6S2dcASRgrcagcfbX6rMq6SA8GqB9pdYmgRwxCnmNdGLYbN

-- Dumped from database version 15.14 (Debian 15.14-1.pgdg13+1)
-- Dumped by pg_dump version 15.14 (Debian 15.14-1.pgdg13+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: VersionInfo; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public."VersionInfo" (
    "Version" bigint NOT NULL,
    "AppliedOn" timestamp without time zone,
    "Description" character varying(1024)
);


ALTER TABLE public."VersionInfo" OWNER TO admin;

--
-- Name: machines; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.machines (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    created_at timestamp without time zone DEFAULT now() NOT NULL,
    updated_at timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.machines OWNER TO admin;

--
-- Name: machines_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.machines ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.machines_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: malfunctions; Type: TABLE; Schema: public; Owner: admin
--

CREATE TABLE public.malfunctions (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    machine_id integer NOT NULL,
    priority integer DEFAULT 1 NOT NULL,
    start_time timestamp without time zone DEFAULT now() NOT NULL,
    end_time timestamp without time zone,
    description text NOT NULL,
    is_resolved boolean DEFAULT false NOT NULL,
    created_at timestamp without time zone DEFAULT now() NOT NULL,
    updated_at timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.malfunctions OWNER TO admin;

--
-- Name: malfunctions_id_seq; Type: SEQUENCE; Schema: public; Owner: admin
--

ALTER TABLE public.malfunctions ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.malfunctions_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: VersionInfo; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public."VersionInfo" ("Version", "AppliedOn", "Description") FROM stdin;
1	2025-09-14 16:13:52.505672	InitialCreate
2	2025-09-14 16:13:52.561148	SeedData
3	2025-09-15 03:51:31.150092	Indexes
\.


--
-- Data for Name: machines; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.machines (id, name, created_at, updated_at) FROM stdin;
1	CNC Machine 1	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
2	CNC Machine 2	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
3	CNC Machine 3	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
5	test	2025-09-14 17:04:57.944345	2025-09-14 17:05:56.58073
7	changed cnc machine name 5	2025-09-14 18:33:53.954245	2025-09-14 18:34:07.708559
\.


--
-- Data for Name: malfunctions; Type: TABLE DATA; Schema: public; Owner: admin
--

COPY public.malfunctions (id, name, machine_id, priority, start_time, end_time, description, is_resolved, created_at, updated_at) FROM stdin;
1	Overheating	1	3	2025-09-14 17:13:52	\N	Temperature 130C exceeds safe limits.	t	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
2	Overheating	2	3	2025-09-14 17:43:52	\N	Temperature 120C exceeds safe limits.	f	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
3	Minor Overheating	3	2	2025-09-14 16:13:52	\N	Temperature 90C exceeds recommended limits.	f	2025-09-14 16:13:52.561148	2025-09-14 16:13:52.561148
4	string	5	1	2025-09-14 19:39:26.711527	\N	string	f	2025-09-14 19:39:26.719069	2025-09-14 19:39:26.719069
\.


--
-- Name: machines_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.machines_id_seq', 7, true);


--
-- Name: malfunctions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: admin
--

SELECT pg_catalog.setval('public.malfunctions_id_seq', 4, true);


--
-- Name: machines PK_machines; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.machines
    ADD CONSTRAINT "PK_machines" PRIMARY KEY (id);


--
-- Name: malfunctions PK_malfunctions; Type: CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.malfunctions
    ADD CONSTRAINT "PK_malfunctions" PRIMARY KEY (id);


--
-- Name: IX_machines_name; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "IX_machines_name" ON public.machines USING btree (name);


--
-- Name: UC_Version; Type: INDEX; Schema: public; Owner: admin
--

CREATE UNIQUE INDEX "UC_Version" ON public."VersionInfo" USING btree ("Version");


--
-- Name: ix_malfunctions_machine_start; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX ix_malfunctions_machine_start ON public.malfunctions USING btree (machine_id, start_time DESC);


--
-- Name: ix_malfunctions_priority_start; Type: INDEX; Schema: public; Owner: admin
--

CREATE INDEX ix_malfunctions_priority_start ON public.malfunctions USING btree (priority, start_time DESC);


--
-- Name: malfunctions FK_malfunctions_machines; Type: FK CONSTRAINT; Schema: public; Owner: admin
--

ALTER TABLE ONLY public.malfunctions
    ADD CONSTRAINT "FK_malfunctions_machines" FOREIGN KEY (machine_id) REFERENCES public.machines(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict x7HeKaWcRKQRy8xv6S2dcASRgrcagcfbX6rMq6SA8GqB9pdYmgRwxCnmNdGLYbN

