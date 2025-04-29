# TODO
- UI 만들기
    - [x] 체력바
    - [x] 현재 장착장비
    - [ ] 퀵슬롯
    - [ ] 건설메뉴
    - [ ] 메인메뉴 (게임종료 등)

- 건설 기능
    - [ ] 청사진효과 (반투명 푸른색 오브젝트로)
    - [ ] 건설시 땅위에 오브젝트 생성

- 상호작용 시스템
    - [ ] 전투
        - [ ] 공격
        - [ ] 피격
        
    - [ ] 채굴
        - [ ] 툴을 이용한 자원채집
        - [ ] 건물을 이용한 자동 채굴
    - [ ] 건물오브젝트와 상호작용
        - [ ] 생산시스템 - 자동화

- [ ] 세이브/로드 시스템

    




# Doing
- [ ] SlotUI 퀵슬롯과의 연계
- [ ] SlotUI 드래그 시, 퀵슬롯 or 인벤토리 판별
- [ ] SlotUI 인벤토리 - 퀵슬롯 간의 아이템 이동 시스템
- [ ] InventoryManager - 싱글톤 해제 ?
- [ ] InventoryManager - 상위 클래스? 또는 Interface로 분할?




# Done
- [ObjectPool] GetObject 메소드에서 prefab의 null 방어

- [EquipmentManager] 장비 아이템 오브젝트 캐싱을 위한 딕셔너리 선언

- [EquipmentManager] EquipWeapon(WeaponItem) -> InstantiateEquipment(EquipmentItem) 으로 무기 장비 뿐만 아니라 부모 클래스인 EquipmentItem의 인스턴스화를 처리

- [InventoryManager] AddItem 메소드 개선

- 아이템 장착시, 인벤토리에서 장착한 아이템 삭제하기

- [x] 아이템 장착시, 인벤토리에서 장착한 아이템이 사라진다. 그러나 장착 아이템 앞 칸에 있는 countable아이템이 그 자리에 복사되어 생김  도대체 왜???
