# KyomuMemoServer
KyomMemoServerで使用するPostgreSQLのDB構造の説明.

- データベース名:kyomudb

- ユーザー管理テーブル:usertable
  - userid text
  - username text
  
- 付箋管理テーブル:fusentable
  - userid text
  - fusenid text
  - tag text[]
  - title text
  - honbun text
  - color text
